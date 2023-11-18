import logging
import os
import uvicorn
from fastapi import FastAPI, Request
from app.routers import orchestration, status
from azure.monitor.opentelemetry import configure_azure_monitor
from opentelemetry import trace
from opentelemetry.propagate import extract

configure_azure_monitor(
    connection_string=os.environ['FoundationaLLM:APIs:LangChainAPI:AppInsightsConnectionString'],
    disable_offline_storage=True,
    logger_name=logging.getLogger(__name__).name
)

app = FastAPI(
    title='FoundationaLLM LangChainAPI',
    summary='API for interacting with language models using LangChain.',
    description='The FoundationaLLM LangChainAPI is a wrapper around LangChain functionality contained in the foundationallm.core Python SDK.',
    version='1.0.0',
    contact={
        'name':'Solliance, Inc.',
        'email':'contact@solliance.net',
        'url':'https://solliance.net/' 
    },
    openapi_url='/swagger/v1/swagger.json',
    docs_url='/swagger',
    redoc_url=None,
    license_info={
        "name": "FoundationaLLM Software License",
        "url": "https://www.foundationallm.ai/license",
    }
)

tracer = trace.get_tracer(__name__, tracer_provider=trace.get_tracer_provider())
logger = logging.getLogger(__name__)
logger.info("LangChainAPI test")

app.include_router(orchestration.router)
app.include_router(status.router)

@app.get('/')
async def root(request: Request):
    """
    Root path of the API.
    
    Returns
    -------
    str
        Returns a JSON object containing a message and value.
    """
    logging.warning('Logging root endpoint warning...')
    
    with tracer.start_as_current_span(
        'langchain_api_root_request',
        context=extract(request.headers),
        kind=trace.SpanKind.SERVER
    ):    
        logger.warning('Logger root endpoint warning...')
        return { 'message': 'This is the Solliance AI Copilot powered by FoundationaLLM!' }

if __name__ == '__main__':
    uvicorn.run('app.main:app', host='0.0.0.0', port=8765, reload=True, forwarded_allow_ips='*', proxy_headers=True)
