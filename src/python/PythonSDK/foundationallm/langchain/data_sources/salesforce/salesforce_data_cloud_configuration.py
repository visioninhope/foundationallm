from typing import List, Optional, Any

from foundationallm.langchain.data_sources import DataSourceConfiguration

class SalesforceDataCloudConfiguration(DataSourceConfiguration):
    """Configuration settings for a connection to SalesForce DataCloud."""
    client_id: str
    client_secret: str
    refresh_token: str
    instance_url: str
    queries : List[Any]
    columns_to_remove : Optional[List[str]]