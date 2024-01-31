from operator import itemgetter

from azure.core.credentials import AzureKeyCredential

from langchain.chains import ConversationalRetrievalChain
from langchain.memory import ConversationBufferMemory
from langchain_community.callbacks import get_openai_callback
from langchain_community.vectorstores.chroma import Chroma
from langchain_core.prompts import PromptTemplate

from foundationallm.config import Configuration
from foundationallm.langchain.agents import AgentBase
from foundationallm.langchain.language_models import LanguageModelBase
from foundationallm.models.orchestration import CompletionRequest, CompletionResponse
from foundationallm.langchain.retrievers import SearchServiceFilterRetriever

class CustomAgent(AgentBase):
    """
    Agent for loading external custom agents
    """

    name : str = "custom_agent"
    module_name : str = "custom_agent"
    class_name : str = "CustomAgent"

    def __init__(
            self,
            completion_request: CompletionRequest,
            llm: LanguageModelBase,
            config: Configuration):
        """
        Initializes a custom agent.

        Parameters
        ----------
        completion_request : CompletionRequest
            The completion request object containing the user prompt to execute, message history,
            and agent and data source metadata.
        llm : LanguageModelBase
            The language model to use for executing the completion request.
        config : Configuration
            Application configuration class for retrieving configuration settings.
        """
        self.prompt_prefix = completion_request.agent.prompt_prefix
        self.prompt_suffix = completion_request.agent.prompt_suffix
        self.message_history = completion_request.message_history
        self.ds_config = completion_request.data_sources[0].configuration

        self.llm = llm.get_completion_model(completion_request.language_model)
        self.embeddings = llm.get_embedding_model(completion_request.embedding_model)

        completion_request.agent.initalize()

        self.llm_chain = completion_request.agent.llm_chain

    def run(self, prompt: str) -> CompletionResponse:
        """
        Executes the custom agent.

        Parameters
        ----------
        prompt : str
            The prompt for which a completion is begin generated.

        Returns
        -------
        CompletionResponse
            Returns a CompletionResponse with completion response,
            the user_prompt, and token utilization and execution cost details.
        """

        with get_openai_callback() as cb:
            return CompletionResponse(
                completion = self.llm_chain.invoke(prompt, return_only_outputs=True)['answer'],
                user_prompt = prompt,
                completion_tokens = cb.completion_tokens,
                prompt_tokens = cb.prompt_tokens,
                total_tokens = cb.total_tokens,
                total_cost = cb.total_cost
            )
