"""
The KnowledgeManagementAgent class is responsible for executing a completion request
over text, whether that be from a user prompt or a RAG pattern, over a vector store.
"""
from typing import List
from langchain_community.callbacks import get_openai_callback
from langchain_core.documents import Document
from langchain_core.language_models import BaseLanguageModel
from langchain_core.prompts import PromptTemplate
from langchain_core.runnables import RunnablePassthrough, RunnableLambda
from langchain_core.output_parsers import StrOutputParser
from foundationallm.config import Configuration
from foundationallm.langchain.language_models import LanguageModelFactory
from foundationallm.langchain.agents.agent_base import AgentBase
from foundationallm.langchain.retrievers import RetrieverFactory
from foundationallm.models.orchestration import KnowledgeManagementCompletionRequest, CompletionResponse

class KnowledgeManagementAgent(AgentBase):
    """
    Agent for pass-through user_prompt or RAG pattern over a vector store.
    """

    def __init__(
            self,
            completion_request: KnowledgeManagementCompletionRequest,            
            config: Configuration,
            resource_provider):
        """
        Initializes a generic knowledge management agent.

        Parameters
        ----------
        completion_request : CompletionRequest
            The completion request object containing the user prompt to execute, message history,
            and agent and data source metadata.
        llm: BaseLanguageModel
            The language model class to use for embedding and completion.
        config : Configuration
            Application configuration class for retrieving configuration settings.
        resource_provider : ResourceProvider
            Resource provider for retrieving embedding and indexing profiles.        
        """
        req_llm = LanguageModelFactory(language_model=completion_request.agent.language_model, config = config).get_llm()
        self.llm = req_llm.get_completion_model(completion_request.agent.language_model)
        self.internal_context = True
        self.agent_prompt = ""
        if completion_request.agent.indexing_profile is not None:
            self.internal_context = False
            retriever_factory = RetrieverFactory(
                            indexing_profile_resource_id = completion_request.agent.indexing_profile,
                            embedding_profile_resource_id= completion_request.agent.embedding_profile,
                            config = config,
                            resource_provider = resource_provider)
            self.retriever = retriever_factory.get_retriever()
            self.agent_prompt = resource_provider.get_resource(completion_request.agent.prompt)
            
        self.full_prompt = ""

    def __format_docs(self, docs:List[Document]) -> str:
        """
        Generates a formatted string from a list of documents for use
        as the context for the completion request.
        """
        return "\n\n".join(doc.page_content for doc in docs)
    
    def __record_full_prompt(self, prompt: str) -> str:
        """
        Records the full prompt for the completion request.

        Parameters
        ----------
        prompt : str
            The prompt that is populated with context.
        
        Returns
        -------
        str
            Returns the full prompt.
        """
        self.full_prompt = prompt
        return prompt

    def run(self, prompt: str) -> CompletionResponse:
        """
        Executes a completion request by querying the vector index with the user prompt.

        Parameters
        ----------
        prompt : str
            The prompt for which a summary completion is begin generated.
        
        Returns
        -------
        CompletionResponse
            Returns a CompletionResponse with the generated summary, the user_prompt,
            generated full prompt with context
            and token utilization and execution cost details.
        """
        with get_openai_callback() as cb:
            prompt_builder = self.agent_prompt
            # default to internal context
            prompt_builder = "{context}"
            first_chain_link = { "context": RunnablePassthrough() }
            # if querying vector store, override
            if self.internal_context == False:
                prompt_builder = self.agent_prompt + \
                        "\n\nQuestion: {question}\n\nContext: {context}\n\nAnswer:"
                first_chain_link = { "context": self.retriever | self.__format_docs, "question": RunnablePassthrough()}        
            prompt_template = PromptTemplate.from_template(prompt_builder)

            # Compose LCEL chain
            chain = (
                first_chain_link
                | prompt_template
                | RunnableLambda(self.__record_full_prompt)
                | self.llm
                | StrOutputParser()
            )

            return CompletionResponse(
                completion = chain.invoke(prompt),
                user_prompt = prompt,
                full_prompt = self.full_prompt.text,
                completion_tokens = cb.completion_tokens,
                prompt_tokens = cb.prompt_tokens,
                total_tokens = cb.total_tokens,
                total_cost = cb.total_cost
            )
