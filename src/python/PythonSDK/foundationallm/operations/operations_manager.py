import json
import requests
from datetime import datetime
from foundationallm.models.operations import OperationStatus
from foundationallm.models.orchestration import CompletionResponse
from foundationallm.operations import LongRunningOperation
from foundationallm.config import Configuration

class OperationsManager():
    """
    Class for managing long running operations.
    """
    def __init__(self, config: Configuration):
        self.config = config
        self.state_api_url = config.get_value('FoundationaLLM:APIs:StateAPI:APIUrl')
        self.state_api_key = config.get_value('FoundationaLLM:APIs:StateAPI:APIKey')
        
    async def create_operation(self, operation_id: str, instance_id: str) -> LongRunningOperation:
        """
        Creates a background operation by settings its initial state through the State API.

        POST {state_api_url}/instances/{instanceId}/operations/{operationId} -> LongRunningOperation
        
        Parameters
        ----------
        operation_id : str
            The unique identifier for the operation.
        instance_id : str
            The unique identifier for the FLLM instance.
        
        Returns
        -------
        LongRunningOperation
            Object representing the operation.
        """
        try:           
            operation = LongRunningOperation(
                operation_id=operation_id,
                status=OperationStatus.PENDING,
                status_message='Operation is pending.',
                last_updated=datetime.now()
            )

            # Call the State API to create a new operation.
            headers = {
                "x-api-key": state_api_key,
                "charset":"utf-8",
                "Content-Type":"application/json"
            }

            r = requests.post(
                f'{state_api_url}/instances/{instance_id}/operations/{operation_id}',
                json=json.dumps(operation),
                headers=headers
            )

            if r.status_code != 202:
                raise Exception(f'Error: ({r.status_code}) {r.text}')
        
            return operation
        except Exception as e:
            # TODO: Log the exception and return an appropriate error message.
            raise e

    async def update_operation(self,
        operation_id: str,
        instance_id: str,
        status: OperationStatus,
        status_message: str,
        completion_response: CompletionResponse) -> LongRunningOperation:
        """
        Updates the state of a background operation through the State API.

        PUT {state_api_url}/instances/{instanceId}/operations/{operationId} -> LongRunningOperation
        
        Parameters
        ----------
        operation : LongRunningOperation
            The operation to update.
        
        Returns
        -------
        LongRunningOperation
            Object representing the operation.
        """
        try:           
            operation = LongRunningOperation(
                operation_id=operation_id,
                status=OperationStatus.PENDING,
                status_message='Operation is pending.',
                last_updated=datetime.now()
            )

            # Call the State API to create a new operation.
            headers = {
                "x-api-key": state_api_key,
                "charset":"utf-8",
                "Content-Type":"application/json"
            }

            r = requests.put(
                f'{state_api_url}/instances/{instance_id}/operations/{operation_id}',
                json=json.dumps(operation),
                headers=headers
            )

            if r.status_code != 202:
                raise Exception(f'Error: ({r.status_code}) {r.text}')

            operation_json = json.loads(r.json())
            operation = LongRunningOperation(**operation_json)
            return operation
        except Exception as e:
            # TODO: Log the exception and return an appropriate error message.
            raise e

    async def get_operation_status(self, operation_id: str, instance_id: str) -> LongRunningOperation:
        """
        Retrieves the state of a background operation through the State API.

        GET {state_api_url}/instances/{instanceId}/operations/{operationId} -> LongRunningOperation
        
        Parameters
        ----------
        operation_id : str
            The unique identifier for the operation.
        
        Returns
        -------
        LongRunningOperation
            Object representing the operation.
        """
        try:
            # Call the State API to create a new operation.
            headers = {
                "x-api-key": state_api_key,
                "charset":"utf-8",
                "Content-Type":"application/json"
            }

            r = requests.get(
                f'{state_api_url}/instances/{instance_id}/operations/{operation_id}',
                headers=headers
            )

            if r.status_code == 404:
                return None

            if r.status_code != 200:
                raise Exception(f'Error: ({r.status_code}) {r.text}')

            operation_json = json.loads(r.json())
            operation = LongRunningOperation(**operation_json)
            return operation
        except Exception as e:
            # TODO: Log the exception and return an appropriate error message.
            raise e

    async def get_operation_result(self, operation_id: str, instance_id: str) -> CompletionResponse:
        """
        Retrieves the result of a background operation through the State API.

        GET {state_api_url}/instances/{instanceId}/operations/{operationId}/result -> CompletionResponse
        
        Parameters
        ----------
        operation_id : str
            The unique identifier for the operation.
        
        Returns
        -------
        CompletionResponse
            Object representing the operation result.
        """
        try:
            # Call the State API to create a new operation.
            headers = {
                "x-api-key": state_api_key,
                "charset":"utf-8",
                "Content-Type":"application/json"
            }

            r = requests.get(
                f'{state_api_url}/instances/{instance_id}/operations/{operation_id}/result',
                headers=headers
            )

            if r.status_code == 404:
                return None

            if r.status_code != 200:
                raise Exception(f'Error: ({r.status_code}) {r.text}')

            completion_json = json.loads(r.json())
            completion = CompletionResponse(**completion_json)
            return completion
        except Exception as e:
