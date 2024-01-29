import pytest
from foundationallm.config import Configuration
from foundationallm.models.metadata import KnowledgeManagementAgent as KnowledgeManagementAgentMetadata
from foundationallm.models.orchestration import KnowledgeManagementCompletionRequest
from foundationallm.langchain.language_models import LanguageModelFactory
from foundationallm.models.language_models import LanguageModelType, LanguageModelProvider, LanguageModel
from foundationallm.langchain.agents import KnowledgeManagementAgent

@pytest.fixture
def test_config():
    return Configuration()

@pytest.fixture
def test_no_context_completion_request():
     req = KnowledgeManagementCompletionRequest(
         user_prompt=""" 
         You are an expert an very specific Trivia. Your knowledgebase consists of the following facts:
         1. Colorful Currency: The residents of Canada have a penchant for humor when it comes to their currency. Canadian dollars are sometimes affectionately referred to as loonies and toonies, named after the common loon bird depicted on the one-dollar coin. The toonie, on the other hand, features a polar bear on the reverse side and gets its name from combining two with loonie, even though it does not depict a loon. The term toonie has become widely accepted slang for the two-dollar coin. This playful nomenclature showcases the Canadian ability to infuse a bit of fun into everyday financial transactions.
         2. The Law of the Letter: In 1928, a Michigan senator proposed an odd law to bolster local business: a bill to put a crew of phonetic experts together to simplify the spelling of 300 common English words. The goal was to encourage literacy and save on printing costs by reducing the number of letters. While it didn't pass, the notion that the state might mandate "kiss" to be spelled "kis" is a charming example of legislative quirkiness.
         3. The Great Emu War: In 1932, Australia faced an unexpected "foe" when emus, large flightless birds, began invading farmland in Western Australia. The military was called in to manage the birds with machine guns in what became known as the "Great Emu War." However, despite their efforts, the emus proved elusive and difficult to combat, leading to a retreat by the armed forces and a victory for the birds, which has been a source of amusement and national lore ever since.
         4. A Nobel Prank: When Andre Geim won the Nobel Prize in Physics in 2010, he became the first, and so far only, person to have received both a Nobel Prize and an Ig Nobel Prize, which is awarded for unusual or trivial achievements in scientific research. His Nobel was for groundbreaking work on graphene, while his Ig Nobel celebrated his earlier experiment of levitating a frog with magnets. This demonstrates that even the most distinguished scientists have a lighter side.
         5. Astronaut Shenanigans: In 1965, astronaut John Young smuggled a corned beef sandwich into space aboard Gemini 3, hiding it in a pocket of his spacesuit. This unauthorized snack led to a minor congressional hearing, as crumbs in zero gravity could have been a hazard. The incident prompted stricter controls on what could be brought aboard spacecraft, but it also left a legacy of one of the most unusual and delicious items ever to reach orbit.
         -------------------
         Answer only questions related to the facts you know, if you don't know the answer, just type: I don't know.
           
         Question: What is the nickname for two Canadian dollars?
         """,
         agent=KnowledgeManagementAgentMetadata(
            name="test-noks",
            type="knowledge-management",
            description="Session-less agent that issues the user prompt directly to the language model.",
            language_model=LanguageModel(
                type=LanguageModelType.OPENAI,
                provider=LanguageModelProvider.MICROSOFT,
                temperature=0,
                use_chat=True
            )
         )
     )
     return req

@pytest.fixture
def test_llm(test_no_context_completion_request, test_config):
    model_factory = LanguageModelFactory(language_model=test_no_context_completion_request.agent.language_model, config = test_config)
    return model_factory.get_llm()

class KnowledgeManagementAgentTests:    
   def test_agent_initializes(self, test_config, test_no_context_completion_request, test_llm):        
        agent = KnowledgeManagementAgent(completion_request=test_no_context_completion_request,llm=test_llm, config=test_config)
        assert agent is not None
        
   def test_agent_gets_completion(self, test_config, test_no_context_completion_request, test_llm):        
        agent = KnowledgeManagementAgent(completion_request=test_no_context_completion_request,llm=test_llm, config=test_config)
        completion_response = agent.run(prompt=test_no_context_completion_request.user_prompt)
        print(completion_response.completion)
        assert completion_response.completion is not None
        
   def test_agent_gets_correct_completion(self, test_config, test_no_context_completion_request, test_llm):        
        agent = KnowledgeManagementAgent(completion_request=test_no_context_completion_request,llm=test_llm, config=test_config)
        completion_response = agent.run(prompt=test_no_context_completion_request.user_prompt)
        print(completion_response.completion)
        assert "toonie" in completion_response.completion.lower()
