import os
import uvicorn
from fastapi import FastAPI
from opentelemetry.instrumentation.fastapi import FastAPIInstrumentor
from opentelemetry.sdk.resources import SERVICE_NAME, Resource
from app.routers import resolve, status

from opentelemetry import trace
from opentelemetry.sdk.trace import TracerProvider
from opentelemetry.sdk.trace.export import (
    BatchSpanProcessor,
    ConsoleSpanExporter,
)

# Sets the global default tracer provider
trace.set_tracer_provider(
TracerProvider(
        resource=Resource.create({SERVICE_NAME: "FoundationaLLM.DataSourceAPI"})
    )
)

# Creates a tracer from the global tracer provider
tracer = trace.get_tracer("FoundationaLLM.DataSourceAPI")

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
    title='FoundationaLLM DataSourceHubAPI',
    summary='API for retrieving DataSource metadata',
    description='The FoundationaLLM DataSourceHubAPI is a wrapper around DataSourceHub functionality contained in the foundationallm.core Python SDK.',
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

FastAPIInstrumentor.instrument_app(app)

app.include_router(resolve.router)
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
    return { 'message': 'FoundationaLLM DataSourceHubAPI' }
    
if __name__ == '__main__':
    uvicorn.run('main:app', host='0.0.0.0', port=8842, reload=True, forwarded_allow_ips='*', proxy_headers=True)