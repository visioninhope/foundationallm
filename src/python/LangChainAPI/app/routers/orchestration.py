"""
The API endpoint for returning the completion from the LLM for the specified user prompt.
"""
import uuid
from typing import Optional
from fastapi import (
    APIRouter,
    BackgroundTasks,
    Body,
    Depends,
    Header,
    HTTPException,
    Request,
    Response,
    status
)
from foundationallm.config import Configuration, Context
from foundationallm.models.orchestration import (
    CompletionRequestBase,    
    CompletionResponse,
    BackgroundOperation,
    BackgroundResponse
)
from foundationallm.models.agents import KnowledgeManagementCompletionRequest
from foundationallm.langchain.orchestration import OrchestrationManager
from foundationallm.telemetry import Telemetry
from app.dependencies import handle_exception, validate_api_key_header

# Initialize telemetry logging
logger = Telemetry.get_logger(__name__)
tracer = Telemetry.get_tracer(__name__)

# Initialize API routing
router = APIRouter(
    prefix='/orchestration',
    tags=['orchestration'],
    dependencies=[Depends(validate_api_key_header)],
    responses={404: {'description':'Not found'}}
)

background_responses = {}

# temporary to support legacy agents alongside the knowledge-management and internal context agent
async def resolve_completion_request(request_body: dict = Body(...)) -> CompletionRequestBase:   
    agent_type = request_body.get("agent", {}).get("type", None)    
    
    match agent_type:
        case "knowledge-management" | "internal-context":
            request = KnowledgeManagementCompletionRequest(**request_body)
            request.agent.type = "knowledge-management"
            return request
        case _:
            raise ValueError(f"Unsupported agent type: {agent_type}")

@router.post(
    '/instances/{instance_id}/completions/operations',
    summary = 'Submit a completion request.',
    status_code = status.HTTP_202_ACCEPTED,
    responses = {
        202: {'description': 'Completion request accepted.'},
    }
)
async def start_completion_operation(
    raw_request: Request,
    background_tasks: BackgroundTasks,
    instance_id: str,
    completion_request: CompletionRequestBase = Depends(resolve_completion_request),
    x_user_identity: Optional[str] = Header(None)
) -> BackgroundOperation:
    """
    Initiates the creation of a completion response in the background.
    
    Returns
    -------
    BackgroundOperation
        Object containing the operation ID.
    """
    with tracer.start_as_current_span('completion') as span:
        try:
            operation_id = str(uuid.uuid4())
            span.set_attribute('operation_id', operation_id)
            span.set_attribute('request_id', completion_request.request_id)
            span.set_attribute('user_identity', x_user_identity)
            
            background_tasks.add_task(
                create_completion_response,
                operation_id,
                completion_request,
                raw_request.app.extra['config'],
                x_user_identity
            )
        
            background_responses[operation_id] = BackgroundResponse(
                operation_id = operation_id,
                completed = False,
                response = None
            )

            return BackgroundOperation(operation_id=operation_id)
    
        except Exception as e:
         handle_exception(e)

@router.get(
    '/instances/{instance_id}/completions/operations/{operation_id}',
    summary = 'Retrieve the completion response for a specified operation ID.',
    responses = {
        200: {'description': 'Successfully retrieved the completion response.'},
        204: {'description': 'The operation has not completed.'},
        404: {'description': 'The operation was not found.'}
    }
)
async def get_completion_operation(
    raw_request: Request,
    instance_id: str,
    operation_id: str
) -> CompletionResponse:
    print('GET COMPLETION:', operation_id)

    if operation_id not in background_responses:
        raise HTTPException(status_code=404)

    background_response = background_responses[operation_id]

    if not background_response.completed:
        return Response(status_code=204)

    return background_response.response

async def create_completion_response(
    operation_id: str,
    completion_request: KnowledgeManagementCompletionRequest,
    configuration: Configuration,
    x_user_identity: Optional[str] = Header(None)
):
    try:
        orchestration_manager = OrchestrationManager(
            completion_request = completion_request,
            configuration = configuration,
            context = Context(user_identity=x_user_identity)
        )

        result = await orchestration_manager.invoke(completion_request)

        background_responses[operation_id].response = result
        background_responses[operation_id].completed = True

        print('COMPLETION RESPONSE:', result)
        
    except Exception as e:
        background_responses[operation_id].response = CompletionResponse(
            user_prompt = completion_request.user_prompt,
            completion = 'An error occurred in the external orchestration service will processing the completion request.'
        )
        background_responses[operation_id].completed = True

        print('COMPLETION ERROR:', e)
