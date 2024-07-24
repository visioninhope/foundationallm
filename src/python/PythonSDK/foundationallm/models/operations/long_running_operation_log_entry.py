from pydantic import BaseModel, Field

class LongRunningOperationLogEntry(BaseModel):
    """
    Represents an entry in the operation execution log.
    """
    timestamp: str = Field(description='The timestamp of the log entry.')
    status: str = Field(description='The status of the operation at the time of the log entry.')
    status_message: str = Field(description='The status message of the log entry.')
