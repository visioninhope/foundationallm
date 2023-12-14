"""
The API endpoint for returning the appropriate agent for the specified user prompt.
"""
import json
from typing import Optional
from fastapi import APIRouter, Depends, Header
from opentelemetry import trace
from opentelemetry.trace import SpanKind
from foundationallm.config import Context
from foundationallm.hubs.agent import AgentHub, AgentHubRequest, AgentHubResponse
from foundationallm.models import AgentHint
from app.dependencies import validate_api_key_header, handle_exception

router = APIRouter(
    prefix='/resolve',
    tags=['resolve'],
    dependencies=[Depends(validate_api_key_header)],
    responses={404: {'description':'Not found'}},
    redirect_slashes=False
)

tracer = trace.get_tracer("FoundationaLLM.AgentHubAPI")

@router.post('')
async def resolve(request: AgentHubRequest, x_user_identity: Optional[str] = Header(None),
                  x_agent_hint: str = Header(None)) -> AgentHubResponse:
    """
    Resolves the best agent to use for the specified user prompt.

    Parameters
    ----------
    request : AgentHubRequest
        The request object containing the user prompt to use in resolving the best agent to return.
    x_user_identity : str
        The optional X-USER-IDENTITY header value.
    x_agent_hint : str
        The optional X-AGENT-HINT header value.
    
    Returns
    -------
    AgentHubResponse
        Object containing the metadata for the resolved agent.
    """
    try:
        with tracer.start_span(name="Resolve", kind=SpanKind.CONSUMER) as root_span:
            
            root_span.set_attribute("x_user_identity", x_user_identity)
            jData = json.loads(x_user_identity)
            root_span.set_attribute("User", jData["upn"])

            context = Context(user_identity=x_user_identity)
            if x_agent_hint is not None and len(x_agent_hint.strip()) > 0:
                agent_hint = AgentHint.model_validate_json(x_agent_hint)
                return AgentHub().resolve(request=request, user_context=context, hint=agent_hint)
            return AgentHub().resolve(request=request, user_context=context)
    except Exception as e:
        handle_exception(e)
