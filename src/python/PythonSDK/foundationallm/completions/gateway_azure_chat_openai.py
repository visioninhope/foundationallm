"""Azure OpenAI chat wrapper."""
from __future__ import annotations

import logging
import os
from typing import Any, Callable, Dict, List, Union, Optional
import openai

from langchain_core.outputs import ChatResult, ChatGeneration
from langchain_core.pydantic_v1 import BaseModel, Field, root_validator
from langchain_core.utils import get_from_dict_or_env

from langchain_core.callbacks import (
    AsyncCallbackManagerForLLMRun,
    CallbackManagerForLLMRun,
)
from langchain_core.language_models import LanguageModelInput
from langchain_core.language_models.chat_models import (
    BaseChatModel,
    agenerate_from_stream,
    generate_from_stream,
)
from langchain_core.messages import (
    AIMessage,
    AIMessageChunk,
    BaseMessage,
    BaseMessageChunk,
    ChatMessage,
    ChatMessageChunk,
    FunctionMessage,
    FunctionMessageChunk,
    HumanMessage,
    HumanMessageChunk,
    SystemMessage,
    SystemMessageChunk,
    ToolMessage,
    ToolMessageChunk,
)

from langchain_openai import AzureOpenAI, AzureChatOpenAI

from foundationallm.clients import GatewayClient

logger = logging.getLogger(__name__)

class GatewayAzureChatOpenAI(AzureChatOpenAI):
    """`Gateway Azure` Chat Completion API.
    """
    gateway_api_url: Union[str, None] = None
    gateway_api_key: Union[str, None] = None
    azure_endpoint: Union[str, None] = None
    """Your Azure endpoint, including the resource.

        Automatically inferred from env var `AZURE_OPENAI_ENDPOINT` if not provided.

        Example: `https://example-resource.azure.openai.com/`
    """
    deployment_name: Union[str, None] = Field(default=None, alias="azure_deployment")
    """A model deployment.

        If given sets the base client URL to include `/deployments/{azure_deployment}`.
        Note: this means you won't be able to use non-deployment endpoints.
    """
    model: Union[str, None] = None
    model_version: str = ""
    """Legacy, for openai<1.0.0 support."""
    openai_api_type: str = ""
    """Legacy, for openai<1.0.0 support."""
    validate_base_url: bool = True
    """For backwards compatibility. If legacy val openai_api_base is passed in, try to
        infer if it is a base_url or azure_endpoint and update accordingly.
    """

    @classmethod
    def get_lc_namespace(cls) -> List[str]:
        """Get the namespace of the langchain object."""
        return ["langchain", "chat_models", "azure_openai"]

    @root_validator()
    def validate_environment(cls, values: Dict) -> Dict:
        """Validate that api key and python package exists in environment."""
        if values["n"] < 1:
            raise ValueError("n must be at least 1.")
        if values["n"] > 1 and values["streaming"]:
            raise ValueError("n must be 1 when streaming.")

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
            "OPENAI_API_VERSION"
        )
        # Check OPENAI_ORGANIZATION for backwards compatibility.
        values["openai_organization"] = (
            values["openai_organization"]
            or os.getenv("OPENAI_ORG_ID")
            or os.getenv("OPENAI_ORGANIZATION")
        )
        values["azure_endpoint"] = values["azure_endpoint"] or os.getenv(
            "AZURE_OPENAI_ENDPOINT"
        )
        values["azure_ad_token"] = values["azure_ad_token"] or os.getenv(
            "AZURE_OPENAI_AD_TOKEN"
        )

        values["openai_api_type"] = get_from_dict_or_env(
            values, "openai_api_type", "OPENAI_API_TYPE", default="azure"
        )
        values["openai_proxy"] = get_from_dict_or_env(
            values, "openai_proxy", "OPENAI_PROXY", default=""
        )
        # For backwards compatibility. Before openai v1, no distinction was made
        # between azure_endpoint and base_url (openai_api_base).
        openai_api_base = values["openai_api_base"]
        if openai_api_base and values["validate_base_url"]:
            if "/openai" not in openai_api_base:
                raise ValueError(
                    "As of openai>=1.0.0, Azure endpoints should be specified via "
                    "the `azure_endpoint` param not `openai_api_base` "
                    "(or alias `base_url`)."
                )
            if values["deployment_name"]:
                raise ValueError(
                    "As of openai>=1.0.0, if `azure_deployment` (or alias "
                    "`deployment_name`) is specified then "
                    "`base_url` (or alias `openai_api_base`) should not be. "
                    "If specifying `azure_deployment`/`deployment_name` then use "
                    "`azure_endpoint` instead of `base_url`.\n\n"
                    "For example, you could specify:\n\n"
                    'azure_deployment="https://xxx.openai.azure.com/", '
                    'deployment_name="my-deployment"\n\n'
                    "Or you can equivalently specify:\n\n"
                    'base_url="https://xxx.openai.azure.com/openai/deployments/my-deployment"'  # noqa: E501
                )
        client_params = {
            "gateway_api_url": values["gateway_api_url"],
            "gateway_api_key": values["gateway_api_key"],
            "azure_endpoint": values["azure_endpoint"],
            "azure_deployment": values["deployment_name"],
            "model": values["model"],
            "timeout": values["request_timeout"],
            "max_retries": values["max_retries"],
            "default_headers": values["default_headers"],
            "default_query": values["default_query"],
            "http_client": values["http_client"],
        }

        client = GatewayClient(**client_params)

        values["client"] = client.chat
        values["async_client"] = client.chat

        #values["client"] = openai.AzureOpenAI(**client_params).chat.completions
        #values["async_client"] = openai.AsyncAzureOpenAI(**client_params).chat.completions

        return values

    @property
    def _identifying_params(self) -> Dict[str, Any]:
        """Get the identifying parameters."""
        return {**self._default_params}

    @property
    def _llm_type(self) -> str:
        return "azure-openai-chat"

    @property
    def lc_attributes(self) -> Dict[str, Any]:
        return {
            "openai_api_type": self.openai_api_type,
            "openai_api_version": self.openai_api_version,
        }

    def _create_chat_result(self, response: Union[dict, BaseModel]) -> ChatResult:
        if not isinstance(response, dict):
            response = response.dict()
        for res in response["choices"]:
            if res.get("finish_reason", None) == "content_filter":
                raise ValueError(
                    "Azure has not provided the response due to a content filter "
                    "being triggered"
                )
        chat_result = super()._create_chat_result(response)

        if "model" in response:
            model = response["model"]
            if self.model_version:
                model = f"{model}-{self.model_version}"

            if chat_result.llm_output is not None and isinstance(
                chat_result.llm_output, dict
            ):
                chat_result.llm_output["model_name"] = model

        return chat_result

    def _generate(
        self,
        messages: List[BaseMessage],
        stop: Optional[List[str]] = None,
        run_manager: Optional[CallbackManagerForLLMRun] = None,
        stream: Optional[bool] = None,
        **kwargs: Any,
    ) -> ChatResult:
        should_stream = stream if stream is not None else self.streaming

        if should_stream:
            stream_iter = self._stream(
                messages, stop=stop, run_manager=run_manager, **kwargs
            )
            return generate_from_stream(stream_iter)
        message_dicts, params = self._create_message_dicts(messages, stop)
        params = {
            **params,
            **({"stream": stream} if stream is not None else {}),
            **kwargs,
        }

        response = self.client.create(messages=message_dicts, **params)

        return self._create_chat_result(response)

    def _create_chat_result(self,response):

        chat = {
            "text": response.result.completion,
            "message": ChatMessage(content=response.result.completion, role="assistant"),
        }

        data = {
            "generations": [ChatGeneration(**chat)],
            "llm_output": {
                "token_usage" :
                {
                    "prompt_tokens" : response.result.prompt_tokens,
                    "completion_tokens" : response.result.completion_tokens
                },
                "system_fingerprint" : response.result.meta_data["SystemFingerprint"]
            }
        }

        result = ChatResult(**data)

        return result
