from typing import Optional
from pydantic import BaseModel, Field
from foundationallm.models.orchestration import CompletionResponse

class BackgroundResponse(BaseModel):
    operation_id: str = Field(description='The unique identifier for the operation.')
    completed: bool = Field(description='Flag indicating whether the operation has completed.')
    response: Optional[CompletionResponse] = Field(description='The completion response object for the operation.')

    # Use a single BackgroundOperation object to store the operation state?
    # Change completed to status? Accepted, Completed, Failed, InProgress, etc.
    # Change response to result?
