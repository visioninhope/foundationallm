from pydantic import BaseModel, Field
from typing import List
from .execution_log_entry import ExecutionLogEntry

class LongRunningOperationBase(BaseModel):
    """
    Base class for long running operations.
    """
    id: str = Field(description='The unique identifier for the operation.')
    type: str = Field(description='The type of operation.')
    status: str = Field(description='The status of the operation.')
    execution_log: List[ExecutionLogEntry] = Field(description='The execution log for the operation.')
