from pydantic import BaseModel, Field

class ExecutionLogEntry(BaseModel):
    """
    Represents an entry in the execution log.
    """
    timestamp: str = Field(description='The timestamp of the log entry.')
    status: str = Field(description='The status of the operation at the time of the log entry.')
    status_message: str = Field(description='The status message of the log entry.')
