from foundationallm.config import Configuration
from foundationallm.hubs.agent import AgentRepository, AgentResolver
from foundationallm.hubs import HubBase

class AgentHub(HubBase):
    """The AgentHub is responsible for resolving agents."""
    def __init__(self, config):
        # initialize config       
        self.config = config
        super().__init__(resolver=AgentResolver(AgentRepository(self.config)))
     