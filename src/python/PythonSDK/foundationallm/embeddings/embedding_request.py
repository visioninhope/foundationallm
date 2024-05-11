from typing import List

from .text_chunk import TextChunk
from pydantic import BaseModel

class EmbeddingRequest(BaseModel):

    text_chunks: List[TextChunk]
    embedding_model_name: str
