"""
Class name: SearchServiceConfiguration
Description: Azure AI Search index information.
"""
from typing import Optional
from foundationallm.langchain.data_sources import DataSourceConfiguration

class SearchServiceConfiguration(DataSourceConfiguration):
    """
    Connection information indicating the index information
    and the number of results to return in the RAG pattern
    """
    endpoint: str
    key_secret: str
    index_name: str
    top_n: Optional[int] = 3
