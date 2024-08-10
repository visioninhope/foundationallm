"""
Class: OpenAIAssistantsApiService
Description: Integration with the OpenAI Assistants API.
"""
from openai import AzureOpenAI
from foundationallm.config import Configuration
from foundationallm.models.orchestration import CompletionResponse
from foundationallm.models.services.openai_assistants_request import OpenAIAssistantsAPIRequest

class OpenAIAssistantsApiService:
    """
    Integration with the OpenAI Assistants API.
    """

    def __init__(self, config: Configuration, azure_openai_client: AzureOpenAI):
        """
        Initializes an OpenAI Assistants API service.

        Parameters
        ----------
        config : Configuration
            Application configuration class for retrieving configuration settings.
        azure_openai_client : AzureOpenAI
            Azure OpenAI client for interacting with the OpenAI Assistants API.
            TODO: AzureOpenAI extends OpenAI, test with OpenAI client as input at some point, for now just focus on Azure.
        """
        self.config = config
        self.client = azure_openai_client
        
    def run(self, request: OpenAIAssistantsAPIRequest) -> CompletionResponse:
        """
        Creates an OpenAI Assistant Run and executes it.

        Parameters
        ----------
        request : OpenAIAssistantsAPIRequest
            The request to run with the OpenAI Assistants API service.
        """
        print("Entered OpenAIAssistantsAPIService.run method.")

        print("Building the Assistants API request.")
        # Build the OpenAI Assistant request, takes in a dictionary of parameters.
        api_request = {
            "role": "user",
            "thread_id": request.thread_id,
            "content": request.user_prompt
            }

        # If any files exist on the request, add them to the dictionary with the code_interpreter tooling.
        if request.file_id_list:
            api_request["attachments"] = []
            for file_id in request.file_id_list:
                api_request["attachments"].append({                    
                    "file_id": file_id,
                    "tools": [{"type": "code_interpreter"}, {"type": "file_search"}]
                })
        
        # Retrieve the assistant
        print("Retrieving the assistant.")
        assistant = self.client.beta.assistants.retrieve(assistant_id=request.assistant_id)

        # Retrieve the thread
        print("Add User prompt to the thread.")
        thread = self.client.beta.threads.create(messages=[api_request])

        # Create and execute the run
        print("Executing the Assistant run.")
        run = self.client.beta.threads.create_and_run_poll(
            thread_id=request.thread_id,
            assistant_id=request.assistant_id
            )

        # Output
        print(run)
        
        return CompletionResponse(
            operation_id = request.operation_id,
            completion = "WORK IN PROGRESS",
            #citations = citations,
            user_prompt = request.user_prompt,
            # full_prompt = self.full_prompt.text,
            completion_tokens = 0,
            prompt_tokens = 0,
            total_tokens = 0,
            total_cost = 0
        )
