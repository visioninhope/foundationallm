"""
Class Name: KnowledgeManagementAgent

Description: Encapsulates the metadata for the agent
fulfilling the orchestration request.
"""
from typing import Optional
from .agent_base import AgentBase

class KnowledgeManagementAgent(AgentBase):
    """Knowlege Management Agent metadata model."""
    indexing_profile_object_id: Optional[str] = None
    text_embedding_profile_object_id: Optional[str] = None
