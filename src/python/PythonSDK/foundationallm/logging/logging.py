import os
import logging
import urllib.parse

from fastapi import FastAPI, Request

#logging
from opentelemetry.instrumentation.logging import LoggingInstrumentor
from opentelemetry.sdk._logs.export import BatchLogRecordProcessor

from opentelemetry._logs import (
    get_logger_provider,
    set_logger_provider,
)
from opentelemetry.sdk._logs import (
    LoggerProvider,
    LoggingHandler,
)

from azure.monitor.opentelemetry.exporter import AzureMonitorLogExporter

#tracing
from opentelemetry import trace, baggage
from opentelemetry.sdk.trace import TracerProvider
from opentelemetry.sdk.resources import SERVICE_NAME, Resource
from opentelemetry.sdk.trace.export import (
    BatchSpanProcessor,
    ConsoleSpanExporter,
)
from opentelemetry.trace import SpanKind

from azure.monitor.opentelemetry.exporter import AzureMonitorTraceExporter

from foundationallm.config import Configuration

class Logging:

    @staticmethod
    def setup_logging(name : str, config : Configuration):

        LoggingInstrumentor().instrument(set_logging_format=True)

        logger_provider = LoggerProvider(
            resource=Resource.create(
                {
                    SERVICE_NAME: name
                }
            )
        )

        set_logger_provider(logger_provider)

        exporter = AzureMonitorLogExporter.from_connection_string(
            conn_str=config.get_value(["FoundationaLLM:AppInsights:ConnectionString"])
        )

        get_logger_provider().add_log_record_processor(BatchLogRecordProcessor(exporter))

        # Attach LoggingHandler to namespaced logger
        handler = LoggingHandler(level=logging.DEBUG, logger_provider=logger_provider)
        logging.getLogger().addHandler(handler)
        logging.getLogger().setLevel(logging.DEBUG)

    @staticmethod
    def setup_jaeger_tracing(name : str, config : Configuration):

        from opentelemetry.exporter.jaeger.thrift import JaegerExporter

        # create a JaegerExporter
        jaeger_exporter = JaegerExporter(
            # configure agent
            agent_host_name='localhost',
            agent_port=6831,
            # optional: configure also collector
            collector_endpoint=config.get_value(["FoundationaLLM:Jaeger:APIUrl"])
            #collector_endpoint='http://localhost:14268/api/traces?format=jaeger.thrift',
            # username=xxxx, # optional
            # password=xxxx, # optional
            # max_tag_value_length=None # optional
        )

        # Create a BatchSpanProcessor and add the exporter to it
        span_processor = BatchSpanProcessor(jaeger_exporter)

        trace.get_tracer_provider().add_span_processor(span_processor)

    @staticmethod
    def setup_azure_tracing(name : str, config : Configuration):

        azure_exporter = AzureMonitorTraceExporter(
            connection_string=config.get_value(["FoundationaLLM:AppInsights:ConnectionString"])
        )

        # Create a BatchSpanProcessor and add the exporter to it
        azure_span_processor = BatchSpanProcessor(azure_exporter)

        # add to the tracer
        trace.get_tracer_provider().add_span_processor(azure_span_processor)

    @staticmethod
    def setup_tracing(name : str, config : Configuration, use_azure : bool = True, use_jaeger : bool = False):

        # Creates a tracer from the global tracer provider
        tracer = trace.get_tracer(name)

        trace_provider = TracerProvider(
            resource=Resource.create({SERVICE_NAME: name})
        )

        # Sets the global default tracer provider
        trace.set_tracer_provider(trace_provider)

        if ( use_azure):
            Logging.setup_azure_tracing(name)

        if ( use_jaeger):
            Logging.setup_jaeger_tracing(name)

    @staticmethod
    def start_span(tracer_name : str, name : str, kind : SpanKind = SpanKind.CONSUMER, request : Request = None):

        tracer = trace.get_tracer(tracer_name)

        root_span = tracer.start_span(name=name, kind=kind)

        try:
            correlation_context = request.headers["correlation-context"].split(",")

            for item in correlation_context:
                key, value = item.split("=")
                baggage.set_baggage(key, urllib.parse.unquote(value))
                root_span.set_attribute(key, urllib.parse.unquote(value))
        except Exception as e:
            logging.error(f'Error setting correlation context: {e}')

        #root_span.set_attribute("x_user_identity", x_user_identity)
        #jData = json.loads(x_user_identity)
        #root_span.set_attribute("User", jData["upn"])

        return root_span

    #set_global_textmap(TraceContextTextMapPropagator())

    #BaggagePropagator = trace.propagation.baggage.BaggagePropagator()
    #BaggagePropagator.extract(request.headers)

    #prop = TraceContextTextMapPropagator()
    #context = prop.extract(carrier=request.headers)

    #baggage.get_all()

    #carrier = {'traceparent': request.headers["traceparent"]}
    #extracted_context = propagate.extract(carrier)

    #with tracer.start_span(name="Resolve", kind=SpanKind.CONSUMER, context=context) as root_span:
    #with tracer.start_span(name="Resolve", kind=SpanKind.CONSUMER) as root_span:
