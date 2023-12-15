"""
Main entry-point for the FoundationaLLM AgentHubAPI.
Runs web server exposing the API.
"""
import uvicorn
import os
from fastapi import FastAPI
from opentelemetry.instrumentation.fastapi import FastAPIInstrumentor
from app.dependencies import get_config
from app.routers import resolve, status, list_agents
#from azure.monitor.opentelemetry import configure_azure_monitor
from foundationallm.config import Configuration

config = get_config()

app_config = Configuration()

# configure_azure_monitor(
#     connection_string=
#       config.get_value('FoundationaLLM:APIs:AgentHubAPI:AppInsightsConnectionString'),
#     disable_offline_storage=True
# )

from opentelemetry import trace
from opentelemetry.sdk.trace import TracerProvider
from opentelemetry.sdk.resources import SERVICE_NAME, Resource
from opentelemetry.sdk.trace.export import (
    BatchSpanProcessor,
    ConsoleSpanExporter,
)

# Sets the global default tracer provider
trace.set_tracer_provider(
TracerProvider(
        resource=Resource.create({SERVICE_NAME: "FoundationaLLM.AgentHubAPI"})
    )
)

# Creates a tracer from the global tracer provider
tracer = trace.get_tracer("FoundationaLLM.AgentHubAPI")

from opentelemetry.exporter.jaeger.thrift import JaegerExporter

# create a JaegerExporter
jaeger_exporter = JaegerExporter(
    # configure agent
    agent_host_name='localhost',
    agent_port=6831,
    # optional: configure also collector
    collector_endpoint='http://localhost:14268/api/traces?format=jaeger.thrift',
    # username=xxxx, # optional
    # password=xxxx, # optional
    # max_tag_value_length=None # optional
)

# Create a BatchSpanProcessor and add the exporter to it
span_processor = BatchSpanProcessor(jaeger_exporter)

# add to the tracer
trace.get_tracer_provider().add_span_processor(span_processor)

from azure.monitor.opentelemetry.exporter import AzureMonitorTraceExporter

azure_exporter = AzureMonitorTraceExporter(
    connection_string=os.environ["FoundationaLLM:AppInsights:ConnectionString"]
)

# Create a BatchSpanProcessor and add the exporter to it
azure_span_processor = BatchSpanProcessor(azure_exporter)

# add to the tracer
trace.get_tracer_provider().add_span_processor(azure_span_processor)

app = FastAPI(
    title='FoundationaLLM AgentHubAPI',
    summary='API for retrieving Agent metadata',
    description=
        """The FoundationaLLM AgentHubAPI is a wrapper around 
        AgentHub functionality contained in the 
        foundationallm.core Python SDK.""",
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

FastAPIInstrumentor.instrument_app(app, tracer_provider=trace.get_tracer_provider())

app.include_router(resolve.router)
app.include_router(list_agents.router)
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
    return { 'message': 'FoundationaLLM AgentHubAPI' }

if __name__ == '__main__':
    uvicorn.run('app.main:app', host='0.0.0.0', port=8742,
                reload=True, forwarded_allow_ips='*', proxy_headers=True)
