"""
Encapsulates properties useful for calling the OpenAI Assistants API.
"""
from typing import List, Optional
from pydantic import BaseModel

class OpenAIAssistantsAPIRequest(BaseModel):
    """
    Encapsulates properties useful for calling the OpenAI Assistants API.
        assistant_id: str - The ID of the assistant to use.
        thread_id: str - The ID of the conversation thread to use.
        attachments: Optional[List[str]] - The list of OpenAI file IDs to use.
        user_prompt: str - The user prompt/message to send to the assistants API.
    """
    assistant_id: str
    thread_id: str
    attachments: Optional[List[str]] = []
    user_prompt: str
