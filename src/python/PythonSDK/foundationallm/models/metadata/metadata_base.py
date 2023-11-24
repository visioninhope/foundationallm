from pydantic import BaseModel

class MetadataBase(BaseModel):
    """Metadata model base class."""
    name: str
    type: str
    use_cache : bool
    description: str