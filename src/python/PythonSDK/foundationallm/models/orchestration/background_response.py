from typing import Optional
from pydantic import BaseModel
from .completion_response import CompletionResponse

class BackgroundResponse(BaseModel):
    operation_id: str
    completed: bool
    response: Optional[CompletionResponse] = None
