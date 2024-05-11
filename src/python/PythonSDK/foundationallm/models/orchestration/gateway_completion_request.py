import uuid
from typing import List, Optional
from pydantic import BaseModel
from foundationallm.models.orchestration import MessageHistoryItem, OrchestrationSettings, CompletionRequestBase

class GatewayCompletionRequest(CompletionRequestBase):
    """
    Gateway completion request.
    """
    tokens_count : Optional[int] = None
