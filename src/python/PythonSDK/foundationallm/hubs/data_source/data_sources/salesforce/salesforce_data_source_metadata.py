from typing import List, Optional
from .salesforce_authentication_metadata import SalesforceAuthenticationMetadata
from .salesforce_query import SalesforceQuery
from foundationallm.hubs.data_source import DataSourceMetadata

class SalesforceDataSourceMetadata(DataSourceMetadata):    
    authentication: SalesforceAuthenticationMetadata
    query: str
    queries: List[SalesforceQuery]
    columns_to_remove : Optional[List[str]]
    
