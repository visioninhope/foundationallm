from typing import List, Optional
from pydantic import BaseModel
from foundationallm.models.orchestration import MessageHistoryItem

class CompletionRequestBase(BaseModel):
    """
    Base class for completion requests.
    """
    operation_id: str
    session_id: Optional[str] = None
    user_prompt: str
    message_history: Optional[List[MessageHistoryItem]] = []
    attachments: Optional[List[str]] = None
