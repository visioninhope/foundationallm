from foundationallm.config import Configuration
from foundationallm.langchain.agents import AgentFactory, LangChainAgentBase
from foundationallm.models.orchestration import (
    CompletionRequestBase,
    CompletionResponse
)

class OrchestrationManager:
    """Client that acts as the entry point for interacting with the FoundationaLLM Python SDK."""

    def __init__(self,
        completion_request: CompletionRequestBase,
        configuration: Configuration):
        """
        Initializes an instance of the OrchestrationManager.
        
        Parameters
        ----------
        completion_request : CompletionRequest
            The CompletionRequest is the metadata object containing the details needed for
            the OrchestrationManager to assemble an agent.
        context : Context
            The user context under which to execution completion requests.
        """
        self.completion_request = completion_request
        self.agent = self.__create_agent(config=configuration)

    def __create_agent(self, config: Configuration) -> LangChainAgentBase:
        """Creates an agent for executing completion requests."""
        agent_factory = AgentFactory(config=config)
        return agent_factory.get_agent(self.completion_request.agent.type)

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
        Executes an async completion request against the LanguageModel using 
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
