from pydantic import BaseModel
from typing import Optional
from foundationallm.models.language_models import LanguageModelType, LanguageModelProvider

class EmbeddingModel(BaseModel):
    """Embedding model metadata model."""
    type: str = LanguageModelType.OPENAI
    provider: Optional[str] = LanguageModelProvider.MICROSOFT
    deployment_name: Optional[str] = None
    model_name: Optional[str] = "text-embedding-ada-002"
    chunk_size: Optional[int] = 1000
