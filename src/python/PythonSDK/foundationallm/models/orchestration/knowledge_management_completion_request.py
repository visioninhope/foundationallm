"""
Class Name: KnowledgeManagementCompletionRequest
Description: Encapsulates the metadata required to complete a knowledge management orchestration request.
"""
from typing import List, Optional
from pydantic import BaseModel
from foundationallm.models.orchestration import MessageHistoryItem
from foundationallm.models.metadata import KnowledgeManagementAgent

class KnowledgeManagementCompletionRequest(BaseModel):
    """
    Orchestration completion request.
    """
    user_prompt: str
    agent: Optional[KnowledgeManagementAgent] = None   
    message_history: Optional[List[MessageHistoryItem]] = []
