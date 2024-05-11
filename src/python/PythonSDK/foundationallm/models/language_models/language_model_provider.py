from enum import Enum

class LanguageModelProvider(str, Enum):
    """Enumerator of the Language Model providers."""

    MICROSOFT = "microsoft"
    GATEWAY = "gateway"
    OPENAI = "openai"
