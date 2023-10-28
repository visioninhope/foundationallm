from re import A
import pytest
from foundationallm.config import Configuration
from app.main import app
from fastapi.testclient import TestClient

@pytest.fixture
def test_config():
    return Configuration()

@pytest.fixture
def api_key(test_config):
    return test_config.get_value("FoundationaLLM:APIs:AgentHubAPI:APIKey")

@pytest.fixture
def headers(api_key):
    return { "X-API-Key": api_key, "X-User-Identity": '{"name": "Test User","user_name": "testuser@foundationallm.ai","upn": "testuser@foundationallm.ai"}' }

@pytest.fixture
def client():
    return TestClient(app)

class ResolveTests:

    def test_invalid_api_key_should_return_401(self, client):
        response = client.post("/resolve", headers={ "X-API-Key": "invalid" })
        assert response.status_code == 401
        
    def test_x_agent_hint_should_return_desired_agent(self, client, headers):
        headers["X-Agent-Hint"] = "weather"
        response = client.post("/resolve", headers=headers, json={"user_prompt": "Tell me about FoundationaLLM?"})                
        assert response.json()["agent"]["name"] == "weather"
        
    def test_if_x_agent_hint_does_not_exist_should_return_default_agent(self, client, headers):
        headers["X-Agent-Hint"] = "invalid"
        response = client.post("/resolve", headers=headers, json={"user_prompt": "Tell me about FoundationaLLM?"})                
        assert response.json()["agent"]["name"] == "default"
        