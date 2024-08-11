import base64
import requests
from foundationallm.models.attachments import AttachmentProperties
from mimetypes import guess_type
from openai import AzureOpenAI, AsyncAzureOpenAI
from typing import List, Union
from urllib.parse import urlparse, unquote

class ImageAnalysisService:
    """
    Performs image analysis via the Azure OpenAI SDK.
    """
    def __init__(self, client: Union[AzureOpenAI, AsyncAzureOpenAI], deployment_model: str):
        """
        Initializes the ImageAnalysisService.

        Parameters
        ----------
        config : Configuration
            Application configuration class for retrieving configuration settings.
        client : Union[AzureOpenAI, AsyncAzureOpenAI]
            The Azure OpenAI client to use for image analysis.
        """
        self.client = client
        self.deployment_model = deployment_model

    def _get_as_base64(self, mime_type: str, image_url: str) -> str:
        """
        Retrieves an image from its URL and converts it to a base64 string.

        Parameters
        ----------
        mime_type : str
            The mime type of the image.
        image_url : str
            The URL of the image.

        Returns
        -------
        str
            The image as a base64 string.
        """
        return f"data:{mime_type};base64,{base64.b64encode(requests.get(image_url).content).decode('utf-8')}"

    def format_results(self, image_analyses: dict) -> str:
        """
        Formats the image analysis results into a markdown table.

        Parameters
        ----------
        image_analyses : dict
            The dictionary containing the image analysis results.

        Returns
        -------
        str
            The formatted image analysis results.
        """
        formatted_results = f"Analysis of the {len(image_analyses)} attached images are as follows:\n"
        for idx, key in enumerate(image_analyses):
            formatted_results += f"## Image {idx + 1}:\n"
            formatted_results += f"- Name : {key}\n"
            formatted_results += f"- Analysis: {image_analyses[key]}\n\n"
        return formatted_results

    async def aanalyze_images(self, image_attachments: List[AttachmentProperties]) -> dict:
        """
        Get the image analysis results from Azure OpenAI.

        Parameters
        ----------
        image_attachments : List[AttachmentProperties]
            The list containing properties of the images to analyze.
        """
        image_analyses = {}

        for attachment in image_attachments:
            if attachment.content_type.startswith('image/'):
                response = await self.client.chat.completions.create(
                    model=self.deployment_model,
                    messages=[
                        {
                            "role": "system",
                            "content": "You are a helpful assistant who provides information about the data found in images. Provide key insights and analysis about the data in the image. You should provide as many details as possible and be specific. Output the results in a markdown formatted table."
                        },
                        {
                            "role": "user",
                            "content": [
                                {
                                    "type": "text",
                                    "content": "Analyze the image:"
                                },
                                {
                                    "type": "image_url",
                                    "image_url": {
                                        "url": self._get_as_base64(mime_type=attachment.content_type, image_url=attachment.provider_file_name)
                                    }
                                }
                            ]
                        }
                    ],
                    max_tokens=4000,
                    temperature=0.5
                )
                image_analyses[attachment.original_file_name] = response.choices[0].message.content
    
        return image_analyses

    def analyze_images(self, image_attachments: List[AttachmentProperties]) -> dict:
        """
        Get the image analysis results from Azure OpenAI.

        Parameters
        ----------
        image_attachments : List[AttachmentProperties]
            The list containing properties of the images to analyze.
        """
        image_analyses = {}
    
        for attachment in image_attachments:
            if attachment.content_type.startswith('image/'):
                response = self.client.chat.completions.create(
                    model=self.deployment_model,
                    messages=[
                        {
                            "role": "system",
                            "content": "You are a helpful assistant who provides information about the data found in images. Provide key insights and analysis about the data in the image. You should provide as many details as possible and be specific. Output the results in a markdown formatted table."
                        },
                        {
                            "role": "user",
                            "content": [
                                {
                                    "type": "text",
                                    "content": "Analyze the image:"
                                },
                                {
                                    "type": "image_url",
                                    "image_url": {
                                        "url": self._get_as_base64(mime_type=attachment.content_type, image_url=attachment.provider_file_name)
                                    }
                                }
                            ]
                        }
                    ],
                    max_tokens=4000,
                    temperature=0.5
                )
                image_analyses[attachment.original_file_name] = response.choices[0].message.content
    
        return image_analyses
