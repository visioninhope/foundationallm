"""
Class: OpenAIEventHandler
Description: This class is used to handle the events that are triggered by the OpenAI Assistant streaming responses.
"""
from typing import Any
from typing_extensions import override
from openai import AssistantEventHandler

class OpenAIEventHandler(AssistantEventHandler):
    """
    The OpenAIEventHandler class is used to handle the events that are triggered by the OpenAI Assistant streaming responses.
    """
    buffer: str = ""

    @override
    def on_text_created(self, text) -> None:
        self.buffer += f"\n## assistant \n> {text.value}\n"
    
    @override
    def on_text_delta(self, delta: Any, snapshot: Any) -> None:
        """
        This method is called when the OpenAI Assistant sends a text delta.
        """
        self.buffer += delta.value

    def on_tool_call_created(self, tool_call: Any) -> None:
        """
        This method is called when the OpenAI Assistant sends a tool call created event.
        """
        self.buffer += f"\n## assistant \n> {tool_call.type}\n"

    def on_tool_call_delta(self, delta: Any, snapshot: Any) -> None:
        """
        This method is called when the OpenAI Assistant sends a tool call delta.
        """
        if delta.type == 'code_interpreter':
            if delta.code_interpreter.input:
                self.buffer += delta.code_interpreter.input
            if delta.code_interpreter.outputs:
                self.buffer += f"\n\noutput >\n"
                for output in delta.code_interpreter.outputs:
                    if output.type == "logs":
                        self.buffer += f"\n{output.logs}\n"

    def get_buffer(self) -> str:
        """
        Returns the buffer that contains the response from the OpenAI Assistant.
        """
        return self.buffer
