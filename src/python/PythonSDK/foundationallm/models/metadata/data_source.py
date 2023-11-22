from typing import Optional, Union

from .metadata_base import MetadataBase
from foundationallm.langchain.data_sources.csv import CSVConfiguration
from foundationallm.langchain.data_sources.sql import SQLDatabaseConfiguration
from foundationallm.langchain.data_sources.blob import BlobStorageConfiguration
from foundationallm.langchain.data_sources.salesforce import SalesforceDataCloudConfiguration

class DataSource(MetadataBase):
    """Data source metadata model."""
    configuration: Union[CSVConfiguration, SQLDatabaseConfiguration, BlobStorageConfiguration, SalesforceDataCloudConfiguration]
    data_description: Optional[str] = None