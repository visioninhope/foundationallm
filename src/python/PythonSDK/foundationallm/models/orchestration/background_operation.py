from pydantic import BaseModel

class BackgroundOperation(BaseModel):
    operation_id: str = None
