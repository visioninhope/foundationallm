"""Azure OpenAI embeddings wrapper."""
from __future__ import annotations

import os
from typing import Callable, Dict, Optional, Union

import base64
from typing import List, Union, Iterable, cast
from typing_extensions import Literal

import httpx
import openai

from langchain_core.pydantic_v1 import Field, root_validator
from langchain_core.utils import get_from_dict_or_env

from langchain_openai.embeddings.base import OpenAIEmbeddings

from foundationallm.models.embedding import TextChunk, EmbeddingRequest
from foundationallm.clients import GatewayClient

class GatewayAzureEmbeddings(OpenAIEmbeddings):
    """`Azure OpenAI` Embeddings API."""

    gateway_api_key: Union[str, None] = None
    gateway_api_url: Union[str, None] = None
    azure_endpoint: Union[str, None] = None
    """Your Azure endpoint, including the resource.

        Automatically inferred from env var `AZURE_OPENAI_ENDPOINT` if not provided.

        Example: `https://example-resource.azure.openai.com/`
    """
    model: Optional[str]
    deployment: Optional[str] = Field(default=None, alias="azure_deployment")
    """A model deployment.

        If given sets the base client URL to include `/deployments/{azure_deployment}`.
        Note: this means you won't be able to use non-deployment endpoints.
    """
    openai_api_key: Union[str, None] = Field(default=None, alias="api_key")
    """Automatically inferred from env var `AZURE_OPENAI_API_KEY` if not provided."""
    azure_ad_token: Union[str, None] = None
    """Your Azure Active Directory token.

        Automatically inferred from env var `AZURE_OPENAI_AD_TOKEN` if not provided.

        For more:
        https://www.microsoft.com/en-us/security/business/identity-access/microsoft-entra-id.
    """  # noqa: E501
    azure_ad_token_provider: Union[Callable[[], str], None] = None
    """A function that returns an Azure Active Directory token.

        Will be invoked on every request.
    """
    openai_api_version: Optional[str] = Field(default=None, alias="api_version")
    """Automatically inferred from env var `OPENAI_API_VERSION` if not provided."""
    validate_base_url: bool = True

    @root_validator()
    def validate_environment(cls, values: Dict) -> Dict:
        """Validate that api key and python package exists in environment."""
        # Check OPENAI_KEY for backwards compatibility.
        # TODO: Remove OPENAI_API_KEY support to avoid possible conflict when using
        # other forms of azure credentials.
        values["openai_api_key"] = (
            values["openai_api_key"]
            or os.getenv("AZURE_OPENAI_API_KEY")
            or os.getenv("OPENAI_API_KEY")
        )
        values["openai_api_base"] = values["openai_api_base"] or os.getenv(
            "OPENAI_API_BASE"
        )
        values["openai_api_version"] = values["openai_api_version"] or os.getenv(
            "OPENAI_API_VERSION", default="2023-05-15"
        )
        values["openai_api_type"] = get_from_dict_or_env(
            values, "openai_api_type", "OPENAI_API_TYPE", default="azure"
        )
        values["openai_organization"] = (
            values["openai_organization"]
            or os.getenv("OPENAI_ORG_ID")
            or os.getenv("OPENAI_ORGANIZATION")
        )
        values["openai_proxy"] = get_from_dict_or_env(
            values,
            "openai_proxy",
            "OPENAI_PROXY",
            default="",
        )
        values["azure_endpoint"] = values["azure_endpoint"] or os.getenv(
            "AZURE_OPENAI_ENDPOINT"
        )
        values["azure_ad_token"] = values["azure_ad_token"] or os.getenv(
            "AZURE_OPENAI_AD_TOKEN"
        )
        # Azure OpenAI embedding models allow a maximum of 16 texts
        # at a time in each batch
        # See: https://learn.microsoft.com/en-us/azure/ai-services/openai/reference#embeddings
        values["chunk_size"] = min(values["chunk_size"], 16)
        # For backwards compatibility. Before openai v1, no distinction was made
        # between azure_endpoint and base_url (openai_api_base).
        openai_api_base = values["openai_api_base"]
        if openai_api_base and values["validate_base_url"]:
            if "/openai" not in openai_api_base:
                values["openai_api_base"] += "/openai"
                raise ValueError(
                    "As of openai>=1.0.0, Azure endpoints should be specified via "
                    "the `azure_endpoint` param not `openai_api_base` "
                    "(or alias `base_url`). "
                )
            if values["deployment"]:
                raise ValueError(
                    "As of openai>=1.0.0, if `deployment` (or alias "
                    "`azure_deployment`) is specified then "
                    "`openai_api_base` (or alias `base_url`) should not be. "
                    "Instead use `deployment` (or alias `azure_deployment`) "
                    "and `azure_endpoint`."
                )
        client_params = {
            "azure_endpoint": values["azure_endpoint"],
            "azure_deployment": values["deployment"],
            "model": values["model"],
            "timeout": values["request_timeout"],
            "max_retries": values["max_retries"],
            "default_headers": values["default_headers"],
            "default_query": values["default_query"],
            "http_client": values["http_client"],
            "gateway_api_url": values["gateway_api_url"],
            "gateway_api_key": values["gateway_api_key"]
        }

        #values["client"] = openai.AzureOpenAI(**client_params).embeddings
        #values["async_client"] = openai.AsyncAzureOpenAI(**client_params).embeddings

        values["client"] = GatewayClient(**client_params)
        values["async_client"] = GatewayClient(**client_params)

        return values

    @property
    def _llm_type(self) -> str:
        return "azure-openai-chat"

    def embed_documents(
        self, texts: List[str], chunk_size: Optional[int] = 0
    ) -> List[List[float]]:
        """Call out to OpenAI's embedding endpoint for embedding search docs.

        Args:
            texts: The list of texts to embed.
            chunk_size: The chunk size of embeddings. If None, will use the chunk size
                specified by the class.

        Returns:
            List of embeddings, one for each text.
        """
        # NOTE: to keep things simple, we assume the list may contain texts longer
        #       than the maximum context and use length-safe embedding function.
        engine = cast(str, self.deployment)
        return self._get_len_safe_embeddings(texts, engine=engine)

    async def aembed_documents(
        self, texts: List[str], chunk_size: Optional[int] = 0
    ) -> List[List[float]]:
        """Call out to OpenAI's embedding endpoint async for embedding search docs.

        Args:
            texts: The list of texts to embed.
            chunk_size: The chunk size of embeddings. If None, will use the chunk size
                specified by the class.

        Returns:
            List of embeddings, one for each text.
        """
        # NOTE: to keep things simple, we assume the list may contain texts longer
        #       than the maximum context and use length-safe embedding function.
        engine = cast(str, self.deployment)
        return await self._aget_len_safe_embeddings(texts, engine=engine)

    def embed_query(self, text: str) -> List[float]:
        """Call out to OpenAI's embedding endpoint for embedding query text.

        Args:
            text: The text to embed.

        Returns:
            Embedding for the text.
        """
        result = self.embed_documents([text]).text_chunks[0]
        return result.embedding

    async def aembed_query(self, text: str) -> List[float]:
        """Call out to OpenAI's embedding endpoint async for embedding query text.

        Args:
            text: The text to embed.

        Returns:
            Embedding for the text.
        """
        embeddings = await self.aembed_documents([text])
        return embeddings[0]

    def _get_len_safe_embeddings(
        self, texts: List[str], *, engine: str, chunk_size: Optional[int] = None
    ) -> List[List[float]]:

        text_chunks = []

        #TODO - break apart the texts in chunks of chunk_size
        i=1
        for text in texts:
            tc = TextChunk(content=text, position=i)
            i+=1
            text_chunks.append(tc)

        request = EmbeddingRequest(text_chunks=text_chunks, embedding_model_name=self.model)
        res = self.client.get_embedding(request)

        return res
