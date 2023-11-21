from typing import List, Optional
from .salesforce_authentication_metadata import SalesforceAuthenticationMetadata
from foundationallm.hubs.data_source import DataSourceMetadata

class SalesforceDataSourceMetadata(DataSourceMetadata):    
    authentication: SalesforceAuthenticationMetadata
    container: str
    files: Optional[List[str]] = None
    
