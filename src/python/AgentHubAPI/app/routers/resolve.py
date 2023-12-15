"""
The API endpoint for returning the appropriate agent for the specified user prompt.
"""
import json
import urllib.parse
from typing import Optional
from fastapi import APIRouter, Depends, Header
from fastapi import FastAPI, Request
from opentelemetry import baggage, trace
from opentelemetry.trace import SpanKind
from opentelemetry import context, propagate
from opentelemetry.propagate import set_global_textmap
from opentelemetry.propagators.textmap import TextMapPropagator
from opentelemetry.propagators import textmap
from opentelemetry.baggage import get_all, set_baggage
from opentelemetry.trace.propagation.tracecontext import TraceContextTextMapPropagator
from opentelemetry.context import get_current

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
        #set_global_textmap(TraceContextTextMapPropagator())
        
        #BaggagePropagator = trace.propagation.baggage.BaggagePropagator()
        #BaggagePropagator.extract(request.headers)

        #prop = TraceContextTextMapPropagator()
        #context = prop.extract(carrier=request.headers)

        baggage.get_all()
        
        #carrier = {'traceparent': request.headers["traceparent"]}
        #extracted_context = propagate.extract(carrier)
        
        #with tracer.start_span(name="Resolve", kind=SpanKind.CONSUMER, context=context) as root_span:
        with tracer.start_span(name="Resolve", kind=SpanKind.CONSUMER) as root_span:

            correlation_context = request.headers["correlation-context"].split(",")

            for item in correlation_context:
                key, value = item.split("=")
                baggage.set_baggage(key, urllib.parse.unquote(value))
                root_span.set_attribute(key, urllib.parse.unquote(value))
    
            #root_span.set_attribute("x_user_identity", x_user_identity)
            #jData = json.loads(x_user_identity)
            #root_span.set_attribute("User", jData["upn"])

            context = Context(user_identity=x_user_identity)
            if x_agent_hint is not None and len(x_agent_hint.strip()) > 0:
                agent_hint = AgentHint.model_validate_json(x_agent_hint)
                return AgentHub(config=request.app.extra['config']).resolve(request=agent_hub_request, user_context=context, hint=agent_hint)
            return AgentHub(config=request.app.extra['config']).resolve(request=agent_hub_request, user_context=context)
    except Exception as e:
        handle_exception(e)
