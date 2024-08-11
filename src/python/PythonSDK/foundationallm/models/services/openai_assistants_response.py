"""
Encapsulates properties useful for calling the OpenAI Assistants API.
"""
from typing import List, Optional, Union
from pydantic import BaseModel
from foundationallm.models.orchestration import OpenAIFilePathMessageContentItem, OpenAIImageFileMessageContentItem, OpenAITextMessageContentItem

class OpenAIAssistantsAPIResponse(BaseModel):
    """
    Encapsulates properties after parsing the output of the OpenAI Assistants API.
       
    """
    content: Optional[List[Union[OpenAIFilePathMessageContentItem, OpenAIImageFileMessageContentItem, OpenAITextMessageContentItem]]]
    analysis: Optional[str]
    completion_tokens: Optional[int]
    prompt_tokens: Optional[int]
    total_tokens: Optional[int]
