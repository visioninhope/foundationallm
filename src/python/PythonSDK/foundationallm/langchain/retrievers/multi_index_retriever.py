"""
Class: AzureAISearchServiceRetriever
Description: LangChain retriever for Azure AI Search.
"""
import json
from typing import List, Optional, Union, Tuple
from langchain_openai import OpenAIEmbeddings
from langchain_core.callbacks import (
    AsyncCallbackManagerForRetrieverRun,
    CallbackManagerForRetrieverRun,
)
from langchain_core.documents import Document
from langchain_core.retrievers import BaseRetriever
from azure.search.documents import SearchClient
from azure.search.documents.models import VectorizedQuery
from azure.core.credentials import AzureKeyCredential
from azure.identity import DefaultAzureCredential
from foundationallm.models.orchestration import Citation
from .citation_retrieval_base import CitationRetrievalBase

class MultiIndexRetriever(BaseRetriever, CitationRetrievalBase):
    """
    LangChain multi retriever.
    Properties:
        retrievers: List[BaseRetriever] -> List of retrievers to use for completion requests.

    Searches embedding and text fields in the list of retrievers indexes for the top_n most relevant documents.

    Default FFLM document structure (overridable by setting the embedding and text field names):
        {
            "Id": "<GUID>",
            "Embedding": [0.1, 0.2, 0.3, ...], # embedding vector of the Text
            "Text": "text of the chunk",
            "Description": "General description about the source of the text",
            "AdditionalMetadata": "JSON string of metadata"
            "ExternalSourceName": "name and location the text came from, url, blob storage url"
            "IsReference": "true/false if the document is a reference document"
        }
    """

    top_n: int = 10
    retrievers: List[BaseRetriever] = []

    def add_retriever(self, retriever: BaseRetriever):
        """
        Add a retriever to the list of retrievers to use for completion requests.

        Parameters
        ----------
        retriever : BaseRetriever
            The retriever to add to the list of retrievers.
        """
        self.retrievers.append(retriever)

    def get_document_citations(self):

        citations = []

        for retriever in self.retrievers:
            citations.append(retriever.get_document_citations())

        return citations

    def _get_relevant_documents(
        self, query: str, *, run_manager: CallbackManagerForRetrieverRun
    ) -> List[Document]:
        """
        Performs a synchronous hybrid search on Azure AI Search index
        """

        total_results = []

        for retriever in self.retrievers:

            results = retriever.search(
                query=query,
                run_manager=run_manager
            )

            total_results.extend(results)

        for result in total_results:
            metadata = json.loads(result[self.metadata_field_name]) if self.metadata_field_name in result else {}
            document = Document(
                    page_content=result[self.text_field_name],
                    metadata=metadata
            )
            self.search_results.append((result[self.id_field_name], document))

        #sort by relevance/score/metric
        #TODO

        #take top n of search_results
        self.search_results = self.search_results[:self.top_n]

        return [doc for _, doc in self.search_results]
