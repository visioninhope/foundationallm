from foundationallm.models.operations import LongRunningOperationBase
from foundationallm.models.orchestration import CompletionResponse
from typing import Optional

class CompletionOperation(LongRunningOperationBase):
    """
    Represents a completion operation.
    """
    type: str = 'completion'
    result: Optional[CompletionResponse] = None
