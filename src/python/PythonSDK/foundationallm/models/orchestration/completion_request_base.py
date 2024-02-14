import uuid
from typing import List, Optional
from pydantic import BaseModel
from foundationallm.models.orchestration import MessageHistoryItem

class CompletionRequestBase(BaseModel):
    """
    Orchestration completion request.
    """
    id: Optional[str] = uuid.uuid4().hex
    user_prompt: str    
    message_history: Optional[List[MessageHistoryItem]] = []
