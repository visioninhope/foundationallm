from typing import List, Optional, Dict
from pydantic import BaseModel

from foundationallm.models.orchestration import CompletionResponse

class CompletionResult(BaseModel):
    in_progress: Optional[bool] = False
    cancelled: Optional[bool] = False
    error_message: Optional[str] = False
    operation_id: str = False
    result: Optional[CompletionResponse] = None
    meta_data: Optional[Dict] = None
    token_count: Optional[int] = None

