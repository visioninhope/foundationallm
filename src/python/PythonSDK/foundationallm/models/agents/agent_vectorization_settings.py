"""
Encapsulates properties agent vectorization settings.
"""
from typing import Optional, List
from pydantic import BaseModel

class AgentVectorizationSettings(BaseModel):
    """
    Encapsulates properties for agent vectorization settings.
    """
    indexing_profile_object_ids: Optional[List[str]] = None
    text_embedding_profile_object_id: Optional[str] = None
