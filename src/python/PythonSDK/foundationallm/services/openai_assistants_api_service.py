"""
Class: OpenAIAssistantsApiService
Description: Integration with the OpenAI Assistants API.
"""
from typing import Union
from openai import AsyncAzureOpenAI, AzureOpenAI
from openai.pagination import AsyncCursorPage, SyncCursorPage
from openai.types import FileObject
from openai.types.beta.threads import FileCitationAnnotation, FilePathAnnotation, ImageFileContentBlock, ImageURLContentBlock, Message, TextContentBlock
from openai.types.beta.threads.message import Attachment
from foundationallm.config import Configuration
from foundationallm.models.orchestration.openai_file_path_message_content_item import OpenAIFilePathMessageContentItem
from foundationallm.models.orchestration.openai_image_file_message_content_item import OpenAIImageFileMessageContentItem
from foundationallm.models.orchestration.openai_text_message_content_item import OpenAITextMessageContentItem
from foundationallm.models.services.openai_assistants_request import OpenAIAssistantsAPIRequest
from foundationallm.models.services.openai_assistants_response import OpenAIAssistantsAPIResponse

class OpenAIAssistantsApiService:
    """
    Integration with the OpenAI Assistants API.
    """

    def __init__(self, config: Configuration, azure_openai_client: Union[AzureOpenAI, AsyncAzureOpenAI]):
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
        attachments = self._get_request_attachments(request)        
    
        # Add User prompt to the thread
        message = self.client.beta.threads.messages.create(
            thread_id=request.thread_id,
            role="user",
            content=request.user_prompt,
            attachments=attachments
        )
        
        # Create and execute the run
        print("Executing the Assistant run.")
        run = self.client.beta.threads.runs.create_and_poll(
            thread_id = request.thread_id,
            assistant_id = request.assistant_id
            )

        # Output
        print(run.usage)

        # Retrieve the messages in the thread after the prompt message was appended.
        messages = self.client.beta.threads.messages.list(
                thread_id=request.thread_id, order="asc", after=message.id
        )

        content = self._parse_messages(messages)
        
        return OpenAIAssistantsAPIResponse(
            content = content,
            completion_tokens = run.usage.completion_tokens,
            prompt_tokens = run.usage.prompt_tokens,
            total_tokens = run.usage.total_tokens
        )

    async def arun(self, request: OpenAIAssistantsAPIRequest) -> OpenAIAssistantsAPIResponse:
        """
        Creates an OpenAI Assistant Run and executes it asynchronously.

        Parameters
        ----------
        request : OpenAIAssistantsAPIRequest
            The request to run with the OpenAI Assistants API service.

        Returns
        -------
        OpenAIAssistantsAPIResponse
            The response parsed from the OpenAI Assistants API service response.
        """
        print("Entered OpenAIAssistantsAPIService.arun method.")

        # Process file attachments and assign tools
        attachments = await self._aget_request_attachments(request)        

        print("Add message to thread.")
        # Add User prompt to the thread
        message = await self.client.beta.threads.messages.create(
            thread_id=request.thread_id,
            role="user",
            content=request.user_prompt,
            attachments=attachments
        )
        
        # Create and execute the run
        print("Executing the Assistant run.")
        run = await self.client.beta.threads.runs.create_and_poll(
            thread_id = request.thread_id,
            assistant_id = request.assistant_id
            )

        # Output
        print(run.usage)
        print("Get message list.")
        # Retrieve the messages in the thread after the prompt message was appended.
        messages = await self.client.beta.threads.messages.list(
                thread_id=request.thread_id, order="asc", after=message.id
        )

        content = await self._aparse_messages(messages)
        
        return OpenAIAssistantsAPIResponse(
            content = content,
            completion_tokens = run.usage.completion_tokens,
            prompt_tokens = run.usage.prompt_tokens,
            total_tokens = run.usage.total_tokens
        )

    def _create_attachment_from_fileobject(self, file:FileObject):
        """
        Creates an attachment from a file object.

        Parameters
        ----------
        file : FileObject
            The file object to create an attachment from.

        Returns
        -------
        Attachment
            The attachment created from the file object.
        """
        #Get the filename extension if it exists
        filename_extension = file.filename.split('.')[-1] if '.' in file.filename else None
        file_search_supported_extensions = ["c", "cpp", "cs", "css", "doc", "docx", "html", "java", "js", "json", "md", "pdf", "php", "pptx", "py", "rb", "sh", "tex", "ts", "txt"]
        tools = [{"type": "code_interpreter"}]
        if filename_extension in file_search_supported_extensions:
            tools.append({"type": "file_search"})
        return Attachment(
                    file_id=file.id,
                    tools = tools
                )
 
    def _get_request_attachments(self, request: OpenAIAssistantsAPIRequest):
        """
        Retrieves the attachments from the request.

        Parameters
        ----------
        request : OpenAIAssistantsAPIRequest
            The request to retrieve attachments from.

        Returns
        -------
        List[Attachment]
            The attachments retrieved from the request.
        """
        attachments = []
        if request.attachments:        
            for file_id in request.attachments:
                oai_file = self.client.files.retrieve(file_id)
                attachments.append(
                     self._create_attachment_from_fileobject(oai_file)
                  )
        return attachments

    async def _aget_request_attachments(self, request: OpenAIAssistantsAPIRequest):
        """
        Retrieves the attachments from the request asynchronously.

        Parameters
        ----------
        request : OpenAIAssistantsAPIRequest
            The request to retrieve attachments from.

        Returns
        -------
        List[Attachment]
            The attachments retrieved from the request.
        """
        attachments = []
        if request.attachments:        
            for file_id in request.attachments:
                oai_file = await self.client.files.retrieve(file_id)
                attachments.append(
                     self._create_attachment_from_fileobject(oai_file)
                  )
        return attachments

    def _parse_single_message(self, message: Message):
        """
        Parses a message from the OpenAI Assistants API
        and returns the content items within the message
        along with any annotations.

        Parameters
        ----------
        message : Message
            The message to parse.

        Returns
        -------
        List[MessageContentItemBase]
            The content items within the message.
        """
        ret_content = []
        # for each content item in the message
        for ci in message.content:                
                match ci:
                    case TextContentBlock():
                        text_ci = OpenAITextMessageContentItem(
                            value=ci.text.value
                        )
                        for annotation in ci.text.annotations:
                            match annotation:
                                case FilePathAnnotation():
                                    file_an = OpenAIFilePathMessageContentItem(
                                        file_id=annotation.file_path.file_id,
                                        start_index=annotation.start_index,
                                        end_index=annotation.end_index,
                                        text=annotation.text
                                    )
                                    text_ci.annotations.append(file_an)
                                case FileCitationAnnotation():
                                    file_cit = OpenAIFilePathMessageContentItem(
                                        file_id=annotation.file_citation.file_id,
                                        start_index=annotation.start_index,
                                        end_index=annotation.end_index,
                                        text=annotation.text
                                    )
                                    text_ci.annotations.append(file_cit)
                        ret_content.append(text_ci)
                    case ImageFileContentBlock():
                        ci_img = OpenAIImageFileMessageContentItem(
                            file_id=ci.image_file.file_id
                        )
                        ret_content.append(ci_img)
                    case ImageURLContentBlock():
                        ci_img_url = OpenAIImageFileMessageContentItem(
                            file_url=ci.image_url.url
                        )
                        ret_content.append(ci_img_url)
        return ret_content
       
    
    def _parse_messages(self, messages: SyncCursorPage[Message]):
        """
        Parses the messages from the OpenAI API.

        Parameters
        ----------
        messages : SyncCursorPage[Message]
            The messages to parse.

        Returns
        -------
        List[MessageContentItemBase]
            The content items within the messages.
        """
        ret_content = []
        for msg in messages:
            ret_content.extend(self._parse_single_message(msg))
        return ret_content

    async def _aparse_messages(self, messages: AsyncCursorPage[Message]):
        """
        Parses the messages from the OpenAI API Asynchronously.

        Parameters
        ----------
        messages : AsyncCursorPage[Message]
            The messages to parse.

        Returns
        -------
        List[MessageContentItemBase]
            The content items within the messages
        """
        ret_content = []
        for msg in messages.data:
            ret_content.extend(self._parse_single_message(msg))
        return ret_content
