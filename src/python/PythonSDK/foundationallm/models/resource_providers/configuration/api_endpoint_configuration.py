from pydantic import Field
from typing import List, Optional
from foundationallm.models.resource_providers import ResourceBase
from foundationallm.models.resource_providers.configuration import (
    ConfigurationTypes,
    UrlException
)

class APIEndpointConfiguration(ResourceBase):
    """
    API Endpoint Configuration model.
    """
    type: str = Field(default=ConfigurationTypes.API_ENDPOINT, description="The type of the API endpoint configuration.")
    category: str = Field(description="The category of the API endpoint configuration.")
    authentication_type: str = Field(description="The type of authentication required for accessing the API endpoint.")
    url: str = Field(description="The base URL of the API endpoint.")
    url_exceptions: List[UrlException] = Field(default={}, description="List of URL exceptions for the API endpoint.")
    authentication_parameters: dict = Field(default={}, description="Dictionary with values used for authentication.")
    timeout_seconds: int = Field(default=60, description="The timeout duration in seconds for API calls.")
    retry_strategy_name: str = Field(description="The name of the retry strategy to use for API calls.")
    provider: Optional[str] = Field(description="The provider of the API endpoint.")
    api_version: Optional[str] = Field(description="The version to use when calling the API represented by the endpoint.")
    operation_type: Optional[str] = Field(description="The type of operation the API endpoint is performing.")
