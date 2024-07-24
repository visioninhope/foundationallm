from datetime import datetime
from pydantic import BaseModel, Field

class LongRunningOperation(BaseModel):
    """
    Class representing a long running operation.
    """
    operation_id: str = Field(description='The unique identifier for the operation.')
    status: str = Field(description='The status of the operation.')
    status_message: str = Field(description='The message associated with the operation status.')
    last_updated: datetime = Field(default=datetime.now(), description='The timestamp of the last update to the operation.')
    
