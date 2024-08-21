from foundationallm.config import Configuration
from foundationallm.operations import OperationsManager
from foundationallm.langchain.agents import (
    LangChainAgentBase,
    LangChainKnowledgeManagementAgent,
    LangChainAudioClassifierAgent
)

class AgentFactory:
    """
    Factory to determine which agent to use.
    """
    def get_agent(
        self,
        agent_type: str,
        config: Configuration,
        operations_manager: OperationsManager) -> LangChainAgentBase:
        """
        Retrieves an agent of the the requested type.

        Parameters
        ----------
        agent_type : str
            The type type assign to the agent returned.

        Returns
        -------
        AgentBase
            Returns an agent of the requested type.
        """
        if agent_type is None:
            raise ValueError("Agent not constructed. Cannot access an object of 'NoneType'.")
        match agent_type:
            case 'knowledge-management':
                return LangChainKnowledgeManagementAgent(config=config, operations_manager=operations_manager)
            case 'audio-classification':
                return LangChainAudioClassifierAgent(config=config, operations_manager=operations_manager)
            case _:
                raise ValueError(f'The agent type {agent_type} is not supported.')
