"""
Class: SearchServiceRetriever
Description: LangChain retriever for Azure AI Search.
"""
from langchain.schema import BaseRetriever
from langchain.embeddings.openai import OpenAIEmbeddings
from langchain.callbacks.manager import (
    AsyncCallbackManagerForRetrieverRun,
    CallbackManagerForRetrieverRun,
)
from azure.search.documents import SearchClient
from azure.search.documents.models import VectorizedQuery
from azure.core.credentials import AzureKeyCredential
from typing import List
from langchain.schema.document import Document

class SearchServiceRetriever(BaseRetriever):
    """
    LangChain retriever for Azure AI Search.
    Properties:
        endpoint: str -> Azure AI Search endpoint
        index_name: str -> Azure AI Search index name
        top_n : int -> number of results to return from vector search
        credential: AzureKeyCredential -> Azure AI Search credential
        embedding_model: OpenAIEmbeddings -> OpenAIEmbeddings model

    Expects document structure:
        {
            "id": "<GUID>",
            "source_uri": "name and location the text came from, url, blob storage url",
            "content": "text of the chunk",
            "content_vector": [0.1, 0.2, 0.3, ...]
            "metadata": "JSON string of metadata"                
        }
    """
    endpoint: str
    index_name: str
    top_n : int
    credential: AzureKeyCredential
    embedding_model: OpenAIEmbeddings

    class Config:
        """Configuration for this pydantic object."""        
        arbitrary_types_allowed = True

    def __get_embeddings(self, text: str) -> List[float]:
        """
        Returns embeddings vector for a given text.
        """
        embedding = self.embedding_model.embed_query(text)        
        return embedding

    def _get_relevant_documents(
        self, query: str, *, run_manager: CallbackManagerForRetrieverRun
    ) -> List[Document]:
        """
        Performs a synchronous hybrid search on Azure AI Search index
        document structure:
            {
                "id": "<GUID>",
                "source_uri": "name and location the text came from, url, blob storage url",
                "content": "text of the chunk",
                "content_vector": [0.1, 0.2, 0.3, ...]
                "metadata": "JSON string of metadata"                
            }
        """        
        search_client = SearchClient(self.endpoint, self.index_name, self.credential)
        vector_query = VectorizedQuery(vector=self.__get_embeddings(query),
                                        k_nearest_neighbors=3,
                                        fields="content_vector")
        results = search_client.search(
            search_text=query,
            vector_queries=[vector_query],
            top=self.top_n,
            select=["id", "content", "source_uri"]
        )
        results_list = [
            Document(                
                page_content=result["content"],
                metadata = {"id": result["id"], "source_uri": result["source_uri"]}
            ) for result in results
        ]
        return results_list

    async def _aget_relevant_documents(
        self, query: str, *, run_manager: AsyncCallbackManagerForRetrieverRun
    ) -> List[Document]:
        """
        Performs an asynchronous hybrid search on Azure AI Search index
        NOTE: This functionality is not currently supported in the underlying Azure SDK.
        """
        raise Exception(f"Asynchronous search not supported.")
