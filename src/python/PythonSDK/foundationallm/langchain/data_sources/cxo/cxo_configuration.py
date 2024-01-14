from typing import List
from foundationallm.langchain.data_sources import DataSourceConfiguration

class CXOConfiguration(DataSourceConfiguration):

    """Configuration settings for a stock agent"""

    sources : List[str]

    retriever_mode : str
    load_mode : str

    company : str
