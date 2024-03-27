"""
Encapsulates properties agent vectorization settings.
"""
from typing import Optional

class AgentVectorizationSettings:
    """
    Encapsulates properties for agent vectorization settings.
    """
    indexing_profile_object_id: Optional[str] = None
    text_embedding_profile_object_id: Optional[str] = None
