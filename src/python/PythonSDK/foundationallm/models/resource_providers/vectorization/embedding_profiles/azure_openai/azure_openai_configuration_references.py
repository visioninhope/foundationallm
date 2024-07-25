"""
Class: AzureOpenAIConfigurationReferences
Description:
    Configuration references for an Azure OpenAI text embedding service.
"""
from pydantic import BaseModel
from typing import Optional

class AzureOpenAIConfigurationReferences(BaseModel):
    """
    Configuration references for an Azure OpenAI text embedding service.
    """
    api_key: Optional[str] = "FoundationaLLM:Vectorization:SemanticKernelTextEmbeddingService:APIKey"
    api_version: Optional[str] = "FoundationaLLM:Vectorization:SemanticKernelTextEmbeddingService:APIVersion"
    authentication_type: Optional[str] = "FoundationaLLM:Vectorization:SemanticKernelTextEmbeddingService:AuthenticationType"
    deployment_name: Optional[str] = "FoundationaLLM:Vectorization:SemanticKernelTextEmbeddingService:DeploymentName"
    endpoint: Optional[str] = "FoundationaLLM:Vectorization:SemanticKernelTextEmbeddingService:Endpoint"
