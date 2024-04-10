"""
Class Name: AgentBase

Description: Encapsulates the common metadata for the agent
fulfilling the orchestration request.
"""
from typing import Optional
from foundationallm.models.language_models import LanguageModel
from foundationallm.models.metadata import ConversationHistory, Gatekeeper, MetadataBase

class AgentBase(MetadataBase):
    """ Agent Base metadata model."""
    prompt_object_id: Optional[str] = None
    sessions_enabled: Optional[bool] = False
    conversation_history: Optional[ConversationHistory] = ConversationHistory()
    gatekeeper: Optional[Gatekeeper] = Gatekeeper()
