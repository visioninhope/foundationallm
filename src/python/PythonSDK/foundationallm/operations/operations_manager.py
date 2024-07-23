import json
import requests
from datetime import datetime
from foundationallm.config import Configuration
from foundationallm.models.operations import LongRunningOperation, OperationStatus
from foundationallm.models.orchestration import CompletionResponse

class OperationsManager():
    """
    Class for managing long running operations via calls to the StateAPI.
    """
    def __init__(self, config: Configuration):
        self.config = config
        # Retrieve the State API configuration settings.
        self.state_api_url = config.get_value('FoundationaLLM:APIs:StateAPI:APIUrl').rstrip('/')
        self.state_api_key = config.get_value('FoundationaLLM:APIs:StateAPI:APIKey')
        
    async def create_operation(
        self,
        operation_id: str,
        instance_id: str) -> LongRunningOperation:
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
        operation = LongRunningOperation(
            operation_id=operation_id,
            status=OperationStatus.PENDING,
            status_message='Operation was submitted and is pending execution.',
            last_updated=datetime.now()
        )
                
        try:
            headers = {
                "x-api-key": self.state_api_key,
                "charset":"utf-8",
                "Content-Type":"application/json"
            }

            # Call the State API to create a new operation.
            r = requests.post(
                f'{self.state_api_url}/instances/{instance_id}/operations/{operation_id}',
                json=json.dumps(operation),
                headers=headers
            )

            if r.status_code != 202:
                raise Exception(f'An error occurred while retrieving the result of operation {operation_id}: ({r.status_code}) {r.text}')

            return operation
        except Exception as e:
            raise e

    async def update_operation(self,
        operation_id: str,
        instance_id: str,
        status: OperationStatus,
        status_message: str) -> LongRunningOperation:
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
        operation = LongRunningOperation(
            operation_id=operation_id,
            status=status,
            status_message=status_message,
            last_updated=datetime.now()
        )
                
        try:
            # Call the State API to create a new operation.
            headers = {
                "x-api-key": self.state_api_key,
                "charset":"utf-8",
                "Content-Type":"application/json"
            }

            r = requests.put(
                f'{self.state_api_url}/instances/{instance_id}/operations/{operation_id}',
                json=json.dumps(operation.__dict__, default=str),
                headers=headers
            )

            if r.status_code == 404:
                return None

            if r.status_code != 202:
                raise Exception(f'An error occurred while retrieving the result of operation {operation_id}: ({r.status_code}) {r.text}')

            return operation
        except Exception as e:
            raise e

    async def get_operation_status(
        self,
        operation_id: str,
        instance_id: str) -> LongRunningOperation:
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
                "x-api-key": self.state_api_key,
                "charset":"utf-8",
                "Content-Type":"application/json"
            }

            r = requests.get(
                f'{self.state_api_url}/instances/{instance_id}/operations/{operation_id}',
                headers=headers
            )

            if r.status_code == 404:
                return None

            if r.status_code != 200:
                raise Exception(f'An error occurred while retrieving the result of operation {operation_id}: ({r.status_code}) {r.text}')

            operation_json = json.loads(r.json())
            operation = LongRunningOperation(**operation_json)
            return operation
        except Exception as e:
            raise e

    async def set_operation_result(
        self,
        operation_id: str,
        instance_id: str,
        completion_response: CompletionResponse):
        """
        Sets the result of a completion operation through the State API.

        PUT {state_api_url}/instances/{instanceId}/operations/{operationId}/result -> CompletionResponse
        
        Parameters
        ----------
        operation_id : str
            The unique identifier for the operation.
        completion_response : CompletionResponse
            The result of the operation.
        """
        try:
            # Call the State API to create a new operation.
            headers = {
                "x-api-key": self.state_api_key,
                "charset":"utf-8",
                "Content-Type":"application/json"
            }

            r = requests.put(
                f'{self.state_api_url}/instances/{instance_id}/operations/{operation_id}/result',
                json=json.dumps(completion_response.__dict__, default=str),
                headers=headers
            )

            if r.status_code == 404:
                return None

            if r.status_code != 202:
                raise Exception(f'An error occurred while retrieving the result of operation {operation_id}: ({r.status_code}) {r.text}')

        except Exception as e:
            raise e

    async def get_operation_result(
        self,
        operation_id: str,
        instance_id: str) -> CompletionResponse:
        """
        Retrieves the result of an async completion operation through the State API.

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
                "x-api-key": self.state_api_key,
                "charset":"utf-8",
                "Content-Type":"application/json"
            }

            r = requests.get(
                f'{self.state_api_url}/instances/{instance_id}/operations/{operation_id}/result',
                headers=headers
            )

            if r.status_code == 404:
                return None

            if r.status_code != 200:
                raise Exception(f'An error occurred while retrieving the result of operation {operation_id}: ({r.status_code}) {r.text}')

            completion_json = json.loads(r.json())
            completion = CompletionResponse(**completion_json)
            return completion
        except Exception as e:
            raise e
