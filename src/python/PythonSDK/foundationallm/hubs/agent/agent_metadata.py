from typing import List, Optional
from foundationallm.hubs import Metadata
from foundationallm.models.metadata import LanguageModel
from .agent_type import AgentType

class AgentMetadata(Metadata):
    """Class representing the metadata for an agent."""
    name: str
    description: str
    type: AgentType
    allowed_data_source_names: Optional[List[str]] = None
    language_model: Optional[LanguageModel] = None
    orchestrator: Optional[str] = None