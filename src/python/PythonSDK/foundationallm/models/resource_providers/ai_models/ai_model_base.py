from pydantic import Field
from typing import List, Optional
from foundationallm.models.resource_providers import ResourceBase

class AIModelBase(ResourceBase):
    """
    The base class used for AIModel resources.
    """
    endpoint_object_id: str = Field(description="The object ID of the APIEndpointConfiguration object providing the configuration for the API endpoint used to interact with the model.")
    version: Optional[str] = Field(description="The version of the AI model.")
    deployment_name: Optional[str] = Field(description="The deployment name for the AI model.")
    model_parameters: List[dict] = Field(description="A dictionary containing default values for model parameters.")
