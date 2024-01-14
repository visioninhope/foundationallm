from typing import List
from foundationallm.langchain.data_sources import DataSourceConfiguration
from foundationallm.langchain.data_sources.search_service import SearchServiceConfiguration

class CXOConfiguration(SearchServiceConfiguration):

    """Configuration settings for a stock agent"""
    sources : List[str]
    retriever_mode : str
    company : str
