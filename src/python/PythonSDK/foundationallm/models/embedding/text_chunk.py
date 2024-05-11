from pydantic import BaseModel
from typing import Optional, List

class TextChunk(BaseModel):

    operation_id : Optional[str] = None
    position : Optional[int] = None
    content : Optional[str] = None
    embedding : Optional[List[float]] = None
    tokens_count : Optional[int] = 0
    has_error : bool = False
    error : Optional[str] = None
