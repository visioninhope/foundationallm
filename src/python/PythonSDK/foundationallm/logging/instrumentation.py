
from opentelemetry.instrumentation.auto_instrumentation import sitecustomize
from starlette.middleware.base import BaseHTTPMiddleware

class OpenTelemetryMiddleware(BaseHTTPMiddleware):
    def self.__init__(self, app):
        super().__init__(app)
    async def dispatch(self, req, call_next):
        response = await call_next(req)
        return response