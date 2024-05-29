from io import BytesIO
from msclap import CLAP
import pandas as pd
import torch
from foundationallm.config import Configuration
from foundationallm.storage import BlobStorageManager

class AudioClassifier():

    def __init__(self, similarity_threshold: float = 28.5, configuration: Configuration = None):
        
        config = configuration or Configuration()
        blob_storage_connection_string = config.get_value('FoundationaLLM:Azure:BlobStorage:ConnectionString')
        file_blob = 'peru_samples_with_embeddings.parquet'

        try:
            storage_manager = BlobStorageManager(
                blob_connection_string=blob_storage_connection_string,
                container_name='audio-data',
                authentication_type='connection_string'
            )
        except Exception as e:
            print(f'Error creating blob storage manager.')
            raise e

        try:
            # Load the reference audio embeddings from the parquet file.
            if (storage_manager.file_exists(file_blob) == False):
                print(f'File {file_blob} not found in blob storage.')
                raise FileNotFoundError(f'File {file_blob} not found in blob storage.')
            file = storage_manager.read_file_content(file_blob)
        except Exception as e:
            print(f'Error retrieving the parquet file {file_blob} from storage.')
            raise e

        try:
            pq_file = BytesIO(file)
            df_ref_audio = pd.read_parquet(pq_file)

            df_bff = df_ref_audio[((df_ref_audio.cluster==0)&(df_ref_audio.species_id=='barred_forest_falcon'))|((df_ref_audio.cluster==1)&(df_ref_audio.species_id=='barred_forest_falcon'))]
            self.bff_audio_embeddings = torch.tensor(list(df_bff.embedding.values))

            self.clap_model = CLAP(version='2023', use_cuda=True)
            self.similarity_threshold = similarity_threshold
        except Exception as e:
            print(f'Error retrieving audio embeddings from {file_blob}.')
            raise e

        print(f'Successfully loaded reference audio embeddings from the file "{file_blob}" from blob storage.')

    def predict(self, audio_embeddings: list):
        """
        Predict the species of a bird based on the similarity of input audio embeddings to the binary classification model.
        """
        audio_embeddings = torch.tensor(audio_embeddings, dtype=torch.float64)
        
        # Calculate similarity between the input audio embeddings and the reference audio embeddings.
        similarity = self.clap_model.compute_similarity(audio_embeddings, self.bff_audio_embeddings)
        similarity_max = [max(x) for x in similarity.detach().numpy()][0]

        print(f'Similarity: {similarity_max}')
        
        return 'barred_forest_falcon' if similarity_max > self.similarity_threshold else 'unknown', similarity_max
