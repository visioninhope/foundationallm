"""
Main entry-point for the FoundationaLLM LangChainAPI.
Runs web server exposing the API.
"""
import uvicorn
import os
from fastapi import FastAPI
from opentelemetry.instrumentation.fastapi import FastAPIInstrumentor
from app.dependencies import get_config
from app.routers import orchestration, status
from foundationallm.logging import Logging

title = "FoundationaLLM.LangChainAPI"

app_config = get_config()

Logging.setup_logging(title, app_config)

Logging.setup_tracing(title, app_config ,use_azure=True, use_jaeger=app_config.get_value("FoundationaLLM:Tracing:UseJaeger"))

app = FastAPI(
    title=title,
    summary='API for interacting with large language models using the LangChain orchestrator.',
    description="""The FoundationaLLM LangChainAPI is a wrapper around LangChain functionality
                contained in the foundationallm.core Python SDK.""",
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
        'name': 'FoundationaLLM Software License',
        'url': 'https://www.foundationallm.ai/license',
    },
    config=app_config
)

FastAPIInstrumentor.instrument_app(app)

app.include_router(orchestration.router)
app.include_router(status.router)

@app.get('/')
async def root():
    """
    Root path of the API.

    Returns
    -------
    str
        Returns a JSON object containing a message and value.
    """
    return { 'message': 'FoundationaLLM LangChainAPI' }

if __name__ == '__main__':
    uvicorn.run('app.main:app', host='0.0.0.0', port=8765, reload=True,
                forwarded_allow_ips='*', proxy_headers=True)
