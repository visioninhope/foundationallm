"""
Class: OpenAIAssistantsApiService
Description: Integration with the OpenAI Assistants API.
"""
from openai import AzureOpenAI
from openai.types.beta.threads import TextContentBlock
from openai.types.beta.threads.message import Attachment
from foundationallm.config import Configuration
from foundationallm.models.orchestration.openai_text_message_content_item import OpenAITextMessageContentItem
from foundationallm.models.services.openai_assistants_request import OpenAIAssistantsAPIRequest
from foundationallm.models.services.openai_assistants_response import OpenAIAssistantsAPIResponse

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
        
    def run(self, request: OpenAIAssistantsAPIRequest) -> OpenAIAssistantsAPIResponse:
        """
        Creates an OpenAI Assistant Run and executes it.

        Parameters
        ----------
        request : OpenAIAssistantsAPIRequest
            The request to run with the OpenAI Assistants API service.
        """
        print("Entered OpenAIAssistantsAPIService.run method.")

        # Process file attachments and assign tools
        if request.attachments:
            attachments = []
            for file_id in request.attachments:
                oai_file = self.client.files.retrieve(file_id)
                #Get the filename extension if it exists
                filename_extension = oai_file.filename.split('.')[-1] if '.' in oai_file.filename else None
                file_search_supported_extensions = ["c", "cpp", "cs", "css", "doc", "docx", "html", "java", "js", "json", "md", "pdf", "php", "pptx", "py", "rb", "sh", "tex", "ts", "txt"]
                tools = [{"type": "code_interpreter"}]
                if filename_extension in file_search_supported_extensions:
                    tools.append({"type": "file_search"})
                attachments.append(
                     Attachment(
                         file_id=file_id,
                         tools = tools
                     )
                  )
    
        # Add User prompt to the thread
        print("Add User prompt to the thread.")        
        message = self.client.beta.threads.messages.create(
            thread_id = request.thread_id,
            role = "user",
            content=request.user_prompt,
            attachments= attachments              
            )
        
        # Create and execute the run
        print("Executing the Assistant run.")
        run = self.client.beta.threads.runs.create_and_poll(
            thread_id = request.thread_id,
            assistant_id = request.assistant_id
            )

        # Output
        print(run.usage)

        # Retrieve the messages in the thread after the message appended.
        messages = self.client.beta.threads.messages.list(
                thread_id=request.thread_id, order="asc", after=message.id
        )

        temp_completion = "Work In Progress"
        for msg in messages:
            for ci in msg.content:
                if isinstance(ci, TextContentBlock):
                    temp_completion = ci.text.value
                    break
        
        return OpenAIAssistantsAPIResponse(
            content = [OpenAITextMessageContentItem(
                 value = temp_completion
                )],
            completion_tokens = run.usage.completion_tokens,
            prompt_tokens = run.usage.prompt_tokens,
            total_tokens = run.usage.total_tokens
        )
