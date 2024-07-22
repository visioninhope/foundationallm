from pydantic import BaseModel, Field
from typing import Optional

class OperationState(BaseModel):
    """
    Represents the state of an operation.
    """
    operation_id: str = Field(description='The unique identifier for the operation.')
    status: str = Field(description='The status of the operation.')
    description: Optional[str] = Field(description='The description of the operation status.')
    status_url: Optional[str] = Field(description='The URL to retrieve the operation status.')
    result_url: Optional[str] = Field(description='The URL to retrieve the completion result.')
