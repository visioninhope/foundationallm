"""
The API endpoint for returning the appropriate agent for the specified user prompt.
"""
import logging
from typing import Optional
from fastapi import APIRouter, Depends, Header
from fastapi import FastAPI, Request

from foundationallm.config import Context
from foundationallm.hubs.agent import AgentHub, AgentHubRequest, AgentHubResponse
from foundationallm.models import AgentHint
from app.dependencies import validate_api_key_header, handle_exception
from foundationallm.logging import Logging

router = APIRouter(
    prefix='/resolve',
    tags=['resolve'],
    dependencies=[Depends(validate_api_key_header)],
    responses={404: {'description':'Not found'}},
    redirect_slashes=False
)

@router.post('')
async def resolve(agent_hub_request: AgentHubRequest,  request: Request, x_user_identity: Optional[str] = Header(None),
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

        with Logging.start_span(request.app.title, "resolve", request=request) as root_span:

            agentHub = None

            context = Context(user_identity=x_user_identity)
            if x_agent_hint is not None and len(x_agent_hint.strip()) > 0:
                agent_hint = AgentHint.model_validate_json(x_agent_hint)
                agentHub = AgentHub(config=request.app.extra['config']).resolve(request=agent_hub_request, user_context=context, hint=agent_hint)
            else:
                agentHub = AgentHub(config=request.app.extra['config']).resolve(request=agent_hub_request, user_context=context)

            return agentHub


    except Exception as e:
        handle_exception(e)
