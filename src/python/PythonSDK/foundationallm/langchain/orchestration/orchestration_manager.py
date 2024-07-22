import requests
from foundationallm.config import Configuration, Context
from foundationallm.langchain.agents import AgentFactory, LangChainAgentBase
from foundationallm.models.agents import KnowledgeManagementCompletionRequest
from foundationallm.models.operations import LongRunningOperation
from foundationallm.models.orchestration import (
    CompletionOperation,
    CompletionRequestBase,
    CompletionResponse
)

class OrchestrationManager:
    """Client that acts as the entry point for interacting with the FoundationaLLM Python SDK."""

    def __init__(self,
                 completion_request: CompletionRequestBase,
                 configuration: Configuration,
                 context: Context):
        """
        Initializes an instance of the OrchestrationManager.
        
        Parameters
        ----------
        completion_request : CompletionRequest
            The CompletionRequest is the metadata object containing the details needed for
            the OrchestrationManager to assemble an agent with a language model and data source.
        context : Context
            The user context under which to execution completion requests.
        """
        self.agent = self.__create_agent(
            completion_request = completion_request,
            config = configuration,
            context = context
        )

    def __create_agent(
            self,
            config: Configuration,
            completion_request: CompletionRequestBase,
            context: Context) -> LangChainAgentBase:
        """Creates an agent for executing completion requests."""
        agent_factory = AgentFactory(
            completion_request=completion_request,
            config=config,
            context=context
        )
        return agent_factory.get_agent()

    def invoke(self, request: CompletionRequestBase) -> CompletionResponse:
        """
        Executes a completion request against the LanguageModel using 
        the LangChain agent assembled by the OrchestrationManager.
        
        Parameters
        ----------
        prompt : str
            The prompt for which a completion is being generated.
            
        Returns
        -------
        CompletionResponse
            Object containing the completion response and token usage details.
        """
        return self.agent.invoke(request)

    async def ainvoke(self, request: CompletionRequestBase) -> CompletionResponse:
        """
        Executes a completion request against the LanguageModel using 
        the LangChain agent assembled by the OrchestrationManager.
        
        Parameters
        ----------
        prompt : str
            The prompt for which a completion is being generated.
            
        Returns
        -------
        CompletionResponse
            Object containing the completion response and token usage details.
        """
        completion_response = await self.agent.ainvoke(request)
        return completion_response

    @staticmethod
    async def create_operation(
        operation_id: str,
        instance_id: str,
        completion_request: KnowledgeManagementCompletionRequest,
        config: Configuration) -> LongRunningOperation:
        """
        Creates a background operation by settings its initial state through the State API.
        
        Parameters
        ----------
        operation_id : str
            The unique identifier for the operation.
        """
        print(f'Creating operation: {operation_id}')
        # Call the State API to create a new operation.
        # This should essentially just be a POST request to the State API to create it.

        #payload = {
        #    'operation_id': operation_id,
        #    'completed': False,
        #    'response': None
        #}
        #headers = {"charset": "utf-8", "Content-Type": "application/json"}

        #r = requests.post(
        #    f'{state_api_endpoint}/operations',
        #    json=payload,
        #    headers=headers
        #)

        #if r.status_code != 202:
        #    raise Exception(f'Error: ({r.status_code}) {r.text}')

        completion_operation = OperationState(
            id=operation_id,
            description='The operation has been submitted and is awaiting completion.'
            status='Accepted'
        )

        # completion_operation.execution_log.append(
        #     ExecutionLogEntry(
        #         status='Accepted',
        #         status_message=f'Operation {operation_id} submitted and accepted.',
        #         timestamp=datetime.now()
        #     )
        # )

        return completion_operation

    @staticmethod
    async def get_operation_state(state_endpoint: str, operation_id: str) -> BackgroundResponse:
        """
        Retrieves the state of an operation by its operation ID.
        
        Parameters
        ----------
        operation_id : str
            The unique identifier for the operation.
            
        Returns
        -------
        CompletionResponse
            Object containing the completion response and token usage details.
        """
        #r = requests.get(f'{state_api_endpoint}/operations/{operation_id}')

        # self.agent.get_state(operation_id)

        return BackgroundResponse(
            operation_id = operation_id,
            completed = True,
            response = CompletionResponse(
                completion = 'Test completion',
                citations = [],
                user_prompt = 'Who are you?',
                full_prompt = '',
                completion_tokens = 1,
                prompt_tokens = 1,
                total_tokens = 1,
                total_cost = 1.00
            )
        )    
            
        #return BackgroundResponse(
        #    operation_id = operation_id,
        #    completed = r.json()['completed'],
        #    response = r.json()['response']
        #)
