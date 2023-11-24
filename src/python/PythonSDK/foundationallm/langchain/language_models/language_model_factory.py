from langchain.base_language import BaseLanguageModel
from foundationallm.config import Configuration
from foundationallm.models.language_models import LanguageModelProvider, LanguageModelType
from foundationallm.langchain.language_models.openai import AzureChatModel
from foundationallm.langchain.language_models.openai import AzureTextCompletionModel
from foundationallm.langchain.language_models.openai import OpenAIChatModel
from foundationallm.langchain.language_models.openai import OpenAITextCompletionModel

import foundationallm.models.metadata.language_model as LanguageModel

class LanguageModelFactory:
    """
    Factory class for determine which language models to use.
    """
    
    def __init__(self, language_model: LanguageModel, config: Configuration):
        self.config = config
        self.language_model = language_model
        
    def get_llm(self) -> BaseLanguageModel:
        """
        Retrieves the language model to use for completion requests.
        
        Returns
        -------
        BaseLanguageModel
            Returns the language model to use for completion requests.
        """
        if self.language_model.type == LanguageModelType.OPENAI:
            match self.language_model.provider:
                case LanguageModelProvider.MICROSOFT:
                    if self.language_model.use_chat:
                        return AzureChatModel(language_model = self.language_model, config = self.config)
                    else:
                        return AzureTextCompletionModel(language_model = self.language_model, config = self.config)
                case LanguageModelProvider.OPENAI:
                    if self.language_model.use_chat:
                        return OpenAIChatModel(language_model = self.language_model, config = self.config)
                    else:
                        return OpenAITextCompletionModel(language_model = self.language_model, config = self.config)
