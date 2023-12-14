"""
The API endpoint for returning the completion from the LLM for the specified user prompt.
"""
import logging
import json
from typing import Optional
from fastapi import APIRouter, Depends, Header, HTTPException
from opentelemetry import trace
from opentelemetry.trace import SpanKind
from foundationallm.config import Context
from foundationallm.models.orchestration import CompletionRequest, CompletionResponse
from foundationallm.langchain.orchestration import OrchestrationManager
from app.dependencies import get_config, validate_api_key_header

# Initialize API routing
router = APIRouter(
    prefix='/orchestration',
    tags=['orchestration'],
    dependencies=[Depends(validate_api_key_header)],
    responses={404: {'description':'Not found'}}
)

tracer = trace.get_tracer("FoundationaLLM.DataSourceHubAPI")

@router.post('/completion')
async def get_completion(completion_request: CompletionRequest,
                         x_user_identity: Optional[str] = Header(None)) -> CompletionResponse:
    """
    Retrieves a completion response from a language model.
    
    Parameters
    ----------
    completion_request : CompletionRequest
        The request object containing the metadata required to build a LangChain agent
        and generate a completion.

    Returns
    -------
    CompletionResponse
        Object containing the completion response and token usage details.
    """
    try:
        with tracer.start_span(name="Completion" , kind=SpanKind.CONSUMER) as root_span:
            
            root_span.set_attribute("x_user_identity", x_user_identity)
            jData = json.loads(x_user_identity)
            root_span.set_attribute("User", jData["upn"])

            orchestration_manager = OrchestrationManager(completion_request = completion_request,
                                                        configuration=get_config(),
                                                        context=Context(user_identity=x_user_identity))
            return orchestration_manager.run(completion_request.user_prompt)
    except Exception as e:
        logging.error(e, stack_info=True, exc_info=True)
        raise HTTPException(
            status_code = 500,
            detail = str(e)
        ) from e
