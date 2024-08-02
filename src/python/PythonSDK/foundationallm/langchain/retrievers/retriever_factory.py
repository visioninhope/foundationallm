from typing import List
from azure.identity import DefaultAzureCredential
from langchain_core.retrievers import BaseRetriever
from foundationallm.config import Configuration
from foundationallm.langchain.language_models.openai import OpenAIModel
from foundationallm.models.language_models import EmbeddingModel, LanguageModelType, LanguageModelProvider
from .azure_ai_search_service_retriever import AzureAISearchServiceRetriever
from foundationallm.models.resource_providers.vectorization import AzureAISearchIndexingProfile

class RetrieverFactory:
    """
    Factory class for determine which retriever to use.
    """
    def __init__(
        self,
        indexing_profiles: List[AzureAISearchIndexingProfile],
        text_embedding_profile:str,
        config: Configuration):

        self.config = config
        self.indexing_profiles = indexing_profiles
        self.text_embedding_profile = text_embedding_profile

    def get_retriever(self) -> BaseRetriever:
        """
        Retrieves the retriever to use for completion requests.

        Returns
        -------
        BaseRetriever
            Returns the concrete initialization of a vectorstore retriever.
        """

        """
        credential_type = self.config.get_value(self.indexing_profile.configuration_references.authentication_type)
        credential = None
        if credential_type == "AzureIdentity":            
            credential = DefaultAzureCredential(exclude_environment_credential=True)
        """

        credential = DefaultAzureCredential(exclude_environment_credential=True)

        # NOTE: Support for all other authentication types has been removed.
                     
        # use embedding profile to build the embedding model (currently only supporting Azure OpenAI)      
        e_model = EmbeddingModel(
            type = LanguageModelType.OPENAI,
            provider = LanguageModelProvider.MICROSOFT,
            # the OpenAI model uses config to retrieve the app config values - pass in the keys
            deployment = self.text_embedding_profile.configuration_references.deployment_name,
            api_endpoint = self.text_embedding_profile.configuration_references.endpoint,
            api_key = credential.get_token("https://cognitiveservices.azure.com/.default").token,
            api_version = self.text_embedding_profile.configuration_references.api_version
        )
        oai_model = OpenAIModel(config = self.config)
        embedding_model = oai_model.get_embedding_model(e_model)

        """
        # use indexing profile to build the retriever (current only supporting Azure AI Search)
        top_n = self.indexing_profile.settings.top_n
        filters = self.indexing_profile.settings.filters
        """

        retriever = AzureAISearchServiceRetriever(
            config=self.config,
            indexing_profiles=self.indexing_profiles,
            embedding_model = embedding_model
        )

        return retriever
