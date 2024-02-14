import logging
from opentelemetry import trace
from opentelemetry.trace import Status, StatusCode

class Telemetry:

    @staticmethod
    def get_logger(name: str, level: int = logging.INFO) -> logging.Logger:
        logger = logging.getLogger(name)
        logger.setLevel(level)
        return logger

    @staticmethod
    def get_tracer(name: str):
        return trace.get_tracer(name)

    @staticmethod
    def record_exception(span, ex: Exception):
        span.set_status(Status(StatusCode.ERROR))
        span.record_exception(ex)
