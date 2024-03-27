"""
Class Name: KnowledgeManagementAgent

Description: Encapsulates the metadata for the agent
fulfilling the orchestration request.
"""
from typing import Optional
from .agent_base import AgentBase
from .agent_vectorization_settings import AgentVectorizationSettings

class KnowledgeManagementAgent(AgentBase):
    """Knowlege Management Agent metadata model."""
    vectorization: AgentVectorizationSettings = None
