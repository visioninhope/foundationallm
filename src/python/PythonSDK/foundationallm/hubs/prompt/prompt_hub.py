from foundationallm.config import Configuration
from foundationallm.hubs.prompt import PromptResolver, PromptRepository
from foundationallm.hubs import HubBase

class PromptHub(HubBase):
    """The PromptHub class is responsible for resolving prompts."""
    
    def __init__(self, config=None):

        self.config = config
         
         # initialize config
        if config is None:
            self.config = Configuration()

        super().__init__( resolver=PromptResolver(PromptRepository(self.config),config=self.config))