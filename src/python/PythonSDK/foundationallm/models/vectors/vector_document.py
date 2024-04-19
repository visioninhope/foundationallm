from typing import Any, List
from langchain_core.documents import Document

class VectorDocument : Document

score: float

def __init__(page_content: str, embedding: List[float], metadata: dict = None, **kwargs: Any) -> None:
    pass
