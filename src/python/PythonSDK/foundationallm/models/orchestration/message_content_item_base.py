from pydantic import BaseModel, Field, Extra
from typing import Optional
from .message_content_item_types import MessageContentItemTypes

class MessageContentItemBase(BaseModel):
    """Base message content item model."""
    
    type: Optional[MessageContentItemTypes] = Field(None, alias="type")
    
    class Config:
        use_enum_values = True
        populate_by_name = True
        extra = Extra.forbid
