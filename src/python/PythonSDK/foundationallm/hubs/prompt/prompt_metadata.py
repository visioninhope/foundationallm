from typing import Optional
from foundationallm.hubs import Metadata

class PromptMetadata(Metadata):
    """Class representing the metadata for a prompt."""

    name: str    
    prompt: str
    prompt_suffix: Optional[str] = None