from enum import Enum

class AttachmentProviders(str, Enum):
    """Enumerator of the Attachment Providers."""
    FOUNDATIONALLM = "foundationallm"
    OPENAI_FILESTORE = "openai_filestore"
