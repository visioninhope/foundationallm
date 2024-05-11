import requests
import openai
import json
import os
import httpx

from typing import Union, Mapping, Literal, List, Iterable, Optional, cast
from httpx import Timeout

from openai.types import CreateEmbeddingResponse

from foundationallm.models.orchestration import CompletionRequestBase, CompletionResponse
from foundationallm.embeddings import EmbeddingRequest, TextEmbeddingResult

RAW_RESPONSE_HEADER = "X-Stainless-Raw-Response"
OVERRIDE_CAST_TO_HEADER = "____stainless_override_cast_to"

# default timeout is 10 minutes
DEFAULT_TIMEOUT = httpx.Timeout(timeout=600.0, connect=5.0)
DEFAULT_MAX_RETRIES = 2
DEFAULT_CONNECTION_LIMITS = httpx.Limits(max_connections=1000, max_keepalive_connections=100)

INITIAL_RETRY_DELAY = 0.5
MAX_RETRY_DELAY = 8.0

__version__ = "0.7.0"

class GatewayClient:

    # client options
    api_key: str
    organization: str | None
    project: str | None
    gateway_api_url: str | None
    gateway_api_key: str | None

    def __init__(
        self,
        *,
        gateway_api_url: str | None = None,
        gateway_api_key: str | None = None,
        azure_endpoint: str | None = None,
        azure_deployment: str | None = None,
        model: str | None = None,
        timeout: Union[float, Timeout, None] = DEFAULT_TIMEOUT,
        max_retries: int = DEFAULT_MAX_RETRIES,
        default_headers: Mapping[str, str] | None = None,
        default_query: Mapping[str, object] | None = None,
        # Configure a custom httpx client.
        # We provide a `DefaultHttpxClient` class that you can pass to retain the default values we use for `limits`, `timeout` & `follow_redirects`.
        # See the [httpx documentation](https://www.python-httpx.org/api/#client) for more details.
        http_client: httpx.Client | None = None,
        # Enable or disable schema validation for data returned by the API.
        # When enabled an error APIResponseValidationError is raised
        # if the API responds with invalid data for the expected schema.
        #
        # This parameter may be removed or changed in the future.
        # If you rely on this feature, please open a GitHub issue
        # outlining your use-case to help us decide if it should be
        # part of our public interface in the future.
        _strict_response_validation: bool = False,
    ) -> None:
        """Construct a new synchronous openai client instance.
        """
        #self._default_stream_cls = Stream

        #self.completions = resources.Completions(self)
        #self.chat = resources.Chat(self)
        #self.embeddings = resources.Embeddings(self)
        #self.embeddings = Embeddings(self)
        #self.files = resources.Files(self)
        #self.images = resources.Images(self)
        #self.audio = resources.Audio(self)
        #self.moderations = resources.Moderations(self)
        #self.models = resources.Models(self)
        #self.fine_tuning = resources.FineTuning(self)
        #self.beta = resources.Beta(self)
        #self.batches = resources.Batches(self)
        #self.with_raw_response = OpenAIWithRawResponse(self)
        #self.with_streaming_response = OpenAIWithStreamedResponse(self)

        self.gateway_api_key = gateway_api_key
        self.gateway_api_url = gateway_api_url

    def try_consume_completion(self, model_id, request:CompletionRequestBase):
        url = f'{self.gateway_api_url}/completions/tryconsume'
        headers = {'x-api-key': self.gateway_api_key}
        data = {'text': request}

        response = requests.post(url, headers=headers, json=data)

        return response.json()

    def try_consume_embedding(self, model_id, text):
        url = f'{self.gateway_api_url}/embeddings/tryconsume'
        headers = {'x-api-key': self.gateway_api_key}
        data = {'text': text}

        response = requests.post(url, headers=headers, json=data)

        return response.json()

    def get_completion(self, request:CompletionRequestBase)->CompletionResponse:
        url = f'{self.gateway_api_url}/completions'
        headers = {'x-api-key': self.gateway_api_key}

        data = request.model_dump()

        response = requests.post(url, headers=headers, json=data)

        return response.json()

    def get_completion_result(self, request : CompletionRequestBase)->CompletionResponse:

        url = f'{self.gateway_api_url}/embeddings?operationId={request.operation_id}'
        headers = {'x-api-key': self.gateway_api_key}

        data = request.model_dump()

        response = requests.get(url, headers=headers, verify=False)
        result = CompletionRequestBase(**response.json())

        return result

    def get_embedding(self, request : EmbeddingRequest)->TextEmbeddingResult:

        url = f'{self.gateway_api_url}/embeddings'
        headers = {'x-api-key': self.gateway_api_key}

        data = request.model_dump()

        response = requests.post(url, headers=headers, json=data, verify=False)

        result = TextEmbeddingResult(**response.json())

        while(result.in_progress):
            result = self.get_embedding_result(result)

        return result

    def get_embedding_result(self, request : TextEmbeddingResult)->TextEmbeddingResult:

        url = f'{self.gateway_api_url}/embeddings?operationId={request.operation_id}'
        headers = {'x-api-key': self.gateway_api_key}

        data = request.model_dump()

        response = requests.get(url, headers=headers, verify=False)
        result = TextEmbeddingResult(**response.json())

        return result

    def create(
        self,
        *,
        input: Union[str, List[str], Iterable[int], Iterable[Iterable[int]]],
        model: Union[str, Literal["text-embedding-ada-002", "text-embedding-3-small", "text-embedding-3-large"]],
        dimensions: int = 1532,
        encoding_format: Literal["float", "base64"] = "float",
        user: str = None,
        # Use the following arguments if you need to pass additional parameters to the API that aren't available via kwargs.
        # The extra values given here take precedence over values defined on the client or passed to this method.
        extra_headers: Mapping[str, str] | None = None,
        extra_query: Mapping[str, str] | None = None,
        extra_body: Mapping[str, str] | None = None,
        timeout: float | httpx.Timeout | None = 600,
    ) -> CreateEmbeddingResponse:
        """
        Creates an embedding vector representing the input text.

        Args:
          input: Input text to embed, encoded as a string or array of tokens. To embed multiple
              inputs in a single request, pass an array of strings or array of token arrays.
              The input must not exceed the max input tokens for the model (8192 tokens for
              `text-embedding-ada-002`), cannot be an empty string, and any array must be 2048
              dimensions or less.
              [Example Python code](https://cookbook.openai.com/examples/how_to_count_tokens_with_tiktoken)
              for counting tokens.

          model: ID of the model to use. You can use the
              [List models](https://platform.openai.com/docs/api-reference/models/list) API to
              see all of your available models, or see our
              [Model overview](https://platform.openai.com/docs/models/overview) for
              descriptions of them.

          dimensions: The number of dimensions the resulting output embeddings should have. Only
              supported in `text-embedding-3` and later models.

          encoding_format: The format to return the embeddings in. Can be either `float` or
              [`base64`](https://pypi.org/project/pybase64/).

          user: A unique identifier representing your end-user, which can help OpenAI to monitor
              and detect abuse.
              [Learn more](https://platform.openai.com/docs/guides/safety-best-practices/end-user-ids).

          extra_headers: Send extra headers

          extra_query: Add additional query parameters to the request

          extra_body: Add additional JSON properties to the request

          timeout: Override the client-level default timeout for this request, in seconds
        """
        params = {
            "input": input,
            "model": model,
            "user": user,
            "dimensions": dimensions,
            "encoding_format": encoding_format,
        }

        return None
