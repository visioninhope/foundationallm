from enum import Enum

class AttachmentProviders(str, Enum):
    """Enumerator of the Attachment Providers."""
    FOUNDATIONALLM_ATTACHMENTS = "FoundationaLLM.Attachments"
    FOUNDATIONALLM_AZURE_OPENAI = "FoundationaLLM.AzureOpenAI"
