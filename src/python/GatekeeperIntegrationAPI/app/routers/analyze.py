"""
The API endpoint for analyzing textual content to identify
PII (personally identifiable information) entities.
"""
from fastapi import APIRouter, Depends
from opentelemetry import trace
from opentelemetry.trace import SpanKind
from foundationallm.integration.models import AnalyzeRequest, AnalyzeResponse
from foundationallm.integration.mspresidio import Analyzer
from app.dependencies import validate_api_key_header, handle_exception

router = APIRouter(
    prefix='/analyze',
    tags=['analyze'],
    dependencies=[Depends(validate_api_key_header)],
    responses={404: {'description':'Not found'}},
    redirect_slashes=False
)

tracer = trace.get_tracer("FoundationaLLM.GatekeeperIntegrationAPI")

@router.post('')
async def analyze(request: AnalyzeRequest) -> AnalyzeResponse:
    """
    Analyze textual content to identify PII with the option to
    anonymize.
    
    Returns
    -------
    AnalyzeResponse
        Returns the original content along with a list of PII
        entities identified in the analyzed text.

        If the request includes anonymize=True, the original content
        will be returned anonymized.
    """
    try:
        with tracer.start_span(name="analyze", kind=SpanKind.CONSUMER ) as root_span:
            
            analyzer = Analyzer(request)
            return analyzer.analyze()
    except Exception as e:
        handle_exception(e)
