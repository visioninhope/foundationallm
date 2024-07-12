from pydantic import BaseModel, Field

class BackgroundOperation(BaseModel):
    operation_id: str = Field(description='The unique identifier for the operation.')
