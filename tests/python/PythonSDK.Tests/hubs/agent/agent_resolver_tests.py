import pytest
from foundationallm.config import Configuration
from foundationallm.hubs.agent import AgentResolver, AgentRepository, AgentHubRequest
from unittest.mock import patch

class AgentResolverTests:
    
    def test_agent_hint_flag_disabled_then_return_default_agent(self):
        with patch.object(Configuration, 'get_feature_flag', return_value=False) as mock_method:
            config = Configuration()
            agent_resolver = AgentResolver(repository=AgentRepository(config=config), config=config)
            agent_hub_request = AgentHubRequest(user_prompt="Tell me about FoundationaLLM?")
            agent_hub_response = agent_resolver.resolve(request=agent_hub_request,hint="weather")
            assert agent_hub_response.agent.name == "default"
            
    def test_agent_hint_flag_enabled_then_return_hinted_agent(self):
        with patch.object(Configuration, 'get_feature_flag', return_value=True) as mock_method:
            config = Configuration()
            agent_resolver = AgentResolver(repository=AgentRepository(config=config), config=config)
            agent_hub_request = AgentHubRequest(user_prompt="Tell me about FoundationaLLM?")
            agent_hub_response = agent_resolver.resolve(request=agent_hub_request,hint="weather")
            assert agent_hub_response.agent.name == "weather"
            