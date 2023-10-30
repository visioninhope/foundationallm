from foundationallm.hubs.agent import AgentMetadata, AgentHubRequest, AgentHubResponse
from foundationallm.hubs import Resolver
from typing import List
from foundationallm.context import Context

class AgentResolver(Resolver):
    """
    The AgentResolver class is responsible for resolving a request to a metadata value.
    
    If a hint is provided, the resolver will return the agent with that name.
    """
    def resolve(self, request:AgentHubRequest, user_context:Context=None, hint:str=None) -> AgentHubResponse:
        hint_feature_flag_enabled = self.config.get_feature_flag("FoundationaLLM-AllowAgentHint")
        if hint_feature_flag_enabled and hint is not None:
            try:
                return AgentHubResponse(agent=self.repository.get_metadata_by_name(hint))
            except:
               # There will be an exception if the hinted agent is not found.
               # Continue with the default agent.
               pass
        return AgentHubResponse(agent=self.repository.get_metadata_by_name("default"))