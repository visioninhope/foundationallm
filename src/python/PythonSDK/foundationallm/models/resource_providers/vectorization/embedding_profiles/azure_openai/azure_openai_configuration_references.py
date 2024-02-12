"""
Class: AzureOpenAIConfigurationReferences
Description:
    Configuration references for an Azure OpenAI text embedding service.
"""
from typing import Optional
from foundationallm.models.resource_providers.vectorization import ConfigurationReferencesBase

class AzureOpenAIConfigurationReferences(ConfigurationReferencesBase):
    """
    Configuration references for an Azure OpenAI text embedding service.
    """
    APIKey: Optional[str] = "FoundationaLLM:Vectorization:SemanticKernelTextEmbeddingService:APIKey"
    APIVersion: Optional[str] = "FoundationaLLM:Vectorization:SemanticKernelTextEmbeddingService:APIVersion"
    AuthenticationType: Optional[str] = "FoundationaLLM:Vectorization:SemanticKernelTextEmbeddingService:AuthenticationType"
    DeploymentName: Optional[str] = "FoundationaLLM:Vectorization:SemanticKernelTextEmbeddingService:DeploymentName"
    Endpoint: Optional[str] = "FoundationaLLM:Vectorization:SemanticKernelTextEmbeddingService:Endpoint"
