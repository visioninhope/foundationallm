from typing import List, Optional
from pydantic import BaseModel

from .text_chunk import TextChunk

class TextEmbeddingResult(BaseModel):
    in_progress: Optional[bool] = False
    cancelled: Optional[bool] = False
    error_message: Optional[str] = False
    operation_id: str = False
    text_chunks: Optional[List[TextChunk]] = None
    token_count: Optional[int] = None

