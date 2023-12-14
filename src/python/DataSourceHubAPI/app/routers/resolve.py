"""
The API endpoint for returning the requested data source metadata.
"""
import json
import logging
from typing import Optional
from fastapi import APIRouter, Depends, HTTPException, Header
from opentelemetry import trace
from opentelemetry.trace import SpanKind
from foundationallm.config import Context
from foundationallm.models import AgentHint
from foundationallm.hubs.data_source import (DataSourceHubRequest,
                                             DataSourceHubResponse, DataSourceHub)
from app.dependencies import validate_api_key_header

router = APIRouter(
    prefix='/resolve',
    tags=['resolve'],
    dependencies=[Depends(validate_api_key_header)],
    responses={404: {'description':'Not found'}}
)

tracer = trace.get_tracer("FoundationaLLM.DataSourceHubAPI")

@router.post('')
async def resolve(request:DataSourceHubRequest,
                  x_user_identity: Optional[str] = Header(None),
                  x_agent_hint: str = Header(None)) -> DataSourceHubResponse:
    """
    Resolves the requested data source metadata.

    Parameters
    ----------
    request : DataSourceHubRequest
        The request object containing the data sources to resolve.
    x_user_identity : str
        The optional X-USER-IDENTITY header value.
    x_agent_hint : str
        The optional X-AGENT-HINT header value.
        
    Returns
    -------
    DataSourceHubResponse
        Object containing the metadata for the resolved data source.
    """
    try:
        with tracer.start_span(name="Resolve" , kind=SpanKind.CONSUMER) as root_span:
            
            root_span.set_attribute("x_user_identity", x_user_identity)
            jData = json.loads(x_user_identity)
            root_span.set_attribute("User", jData["upn"])

            context = Context(user_identity=x_user_identity)
            if x_agent_hint is not None and len(x_agent_hint.strip()) > 0:
                agent_hint = AgentHint.model_validate_json(x_agent_hint)
                return DataSourceHub().resolve(request=request, user_context=context, hint=agent_hint)
            return DataSourceHub().resolve(request=request, user_context=context)
    except Exception as e:
        logging.error(e, stack_info=True, exc_info=True)
        raise HTTPException(
            status_code = 500,
            detail = str(e)
        ) from e
