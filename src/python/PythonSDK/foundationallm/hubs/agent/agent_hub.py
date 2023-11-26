from foundationallm.config import Configuration
from foundationallm.hubs.agent import AgentRepository, AgentResolver
from foundationallm.hubs import HubBase

class AgentHub(HubBase):
    """The AgentHub is responsible for resolving agents."""
    def __init__(self, config=None): 

        self.config = config       
        
        # initialize config 
        if (config is None):
            self.config = Configuration()
        
        super().__init__(resolver=AgentResolver(repository=AgentRepository(self.config), config=self.config))
     