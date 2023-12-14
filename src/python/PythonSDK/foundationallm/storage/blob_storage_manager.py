from io import BytesIO, StringIO
import pandas as pd
import pyarrow.parquet as pq
import fnmatch
from azure.storage.blob import BlobServiceClient
from foundationallm.storage import StorageManagerBase

class BlobStorageManager(StorageManagerBase):
    """ The BlobStorageManager class is responsible for managing files in Azure Blob Storage."""

    def __init__(self, blob_connection_string=None, container_name=None):

        if blob_connection_string is None or blob_connection_string == '':
            raise ValueError(
                'The blob_connection_string parameter must be set to a valid connection string.')
        if container_name is None or container_name == '':
            raise ValueError('The container_name parameter must be set to a valid container.')

        self.blob_connection_string = blob_connection_string
        self.container_name = container_name
        self.blob_service_client = \
                    BlobServiceClient.from_connection_string(self.blob_connection_string)
        self.blob_container_client = self.blob_service_client.get_container_client(container_name)


    def __get_full_path(self, path):
        return '/'.join([i for i in path.split('/') if i != ''])

    def list_blobs(self, path, file_name_pattern=None):
        """
        Returns a list of blobs in the specified path. The path should be to a folder, not a file.

        Args:
        file_name_pattern: used to filter the results using a wildcard pattern for the file name.
        Example:
            file_name_pattern='*1980*.csv' returns all blobs that contain '1980' in the file name.
        """
        full_path = self.__get_full_path(path)
        blobs = self.blob_container_client.list_blobs(name_starts_with=full_path)

        if file_name_pattern is None:
            return blobs

        # Filter the blobs by the file name pattern and exclude any folders
        filtered_blobs = [blob for blob in blobs if fnmatch.fnmatch(blob.name, file_name_pattern)
                          and blob.content_settings.content_type is not None]

        return filtered_blobs

    def file_exists(self, path) -> bool:
        full_path = self.__get_full_path(path)
        blob = self.blob_container_client.get_blob_client(full_path)
        return blob.exists()

    def read_file_content(self, path, read_into_stream=True) -> bytes:
        if self.file_exists(path):
            full_path = self.__get_full_path(path)
            if read_into_stream:
                blob = self.blob_container_client.get_blob_client(full_path)
                stream = BytesIO()
                blob.download_blob().readinto(stream)
                return stream.getvalue()
            else:
                blob = self.blob_container_client.download_blob(full_path)
                return blob.content_as_bytes()
        else:
            return None

    def write_file_content(self, path, content, overwrite=True, lease=None):
        full_path = self.__get_full_path(path)
        blob = self.blob_container_client.get_blob_client(full_path)
        blob.upload_blob(content, overwrite=overwrite, lease=lease)

    def delete_file(self, path):
        full_path = self.__get_full_path(path)
        self.blob_container_client.delete_blob(full_path, delete_snapshots='include')

    def read_dataframe(self, path, format='csv', containerName=None, root_path=None, ret=pd.DataFrame()):

        if self.file_exists(path):

            if format == 'csv':
                return pd.read_csv(StringIO(
                    self.read_file_content(path).decode('utf-8')))
            elif format == 'parquet':
                file_content = self.read_file_content(path)
                table = pq.read_table(BytesIO(file_content))
                df = table.to_pandas()
                return df

        return ret
    
    def read_dataframes(self, folder_path, format='csv', ret = pd.DataFrame()):
        """
        Returns a list of dataframes from the specified folder path.
        The path should be to a folder, not a file.
        """

        if format == 'csv':
            file_name_pattern='*.csv'
        elif format == 'parquet':
            file_name_pattern='*.parquet'
        else:
            raise ValueError('The parameter format must be set to either csv or parquet.')

        blobs = self.list_blobs(folder_path, file_name_pattern=file_name_pattern)
        dfs = pd.DataFrame()
        for blob in blobs:
            df = self.read_dataframe(blob.name, format=format)
            dfs = pd.concat([dfs, df])
        return dfs
    
    def write_dataframe(self, path, df, format='csv', overwrite=True,remove_duplicate_columns=False, lease=None):

        if ( df is None):
            df = pd.DataFrame()

        if ( remove_duplicate_columns):
            df = df.loc[:,~df.columns.duplicated()].copy()

        if format == 'csv':
            self.write_file_content(path,
                df.to_csv(index=False),
                overwrite=overwrite,
                lease=lease)
        elif format == 'parquet':
            self.write_file_content(path,
                df.to_parquet(engine = 'pyarrow', index=False),
                overwrite=overwrite)