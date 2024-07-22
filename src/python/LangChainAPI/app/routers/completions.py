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
from foundationallm.models.operations import OperationState
from foundationallm.models.orchestration import (
    CompletionOperation,
    CompletionRequestBase,    
    CompletionResponse
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
    prefix='/instances/{instance_id}',
    tags=['completions'],
    dependencies=[Depends(validate_api_key_header)],
    responses={404: {'description':'Not found'}}
)

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
    '/async-completions',
    summary = 'Submit a completion request operation.',
    status_code = status.HTTP_202_ACCEPTED,
    responses = {
        202: {'description': 'Completion request accepted.'},
    }
)
async def submit_completion_request(
    response: Response,
    raw_request: Request,
    background_tasks: BackgroundTasks,
    instance_id: str,
    completion_request: CompletionRequestBase = Depends(resolve_completion_request),
    x_user_identity: Optional[str] = Header(None)
) -> OperationState:
    """
    Initiates the creation of a completion response in the background.
    
    Returns
    -------
    CompletionOperation
        Object containing the operation ID and status.
    """
    with tracer.start_as_current_span('async-completions') as span:
        try:
            operation_id = str(uuid.uuid4())
            span.set_attribute('operation_id', operation_id)
            span.set_attribute('instance_id', instance_id)
            span.set_attribute('request_id', completion_request.request_id)
            span.set_attribute('user_identity', x_user_identity)

            # Add a location header to the response.
            response.headers['location'] = f'{raw_request.base_url}instances/{instance_id}/async-completions/{operation_id}'

            # Kick of the background task to create the completion response.
            background_tasks.add_task(
                create_completion_response,
                operation_id,
                instance_id,
                completion_request,
                raw_request.app.extra['config'],
                x_user_identity
            )

            # Submit the completion request operation to the state API.
            operation_state = await OrchestrationManager.create_operation(
                operation_id,
                instance_id,
                completion_request,
                raw_request.app.extra['config'],
                x_user_identity
            )

            # Append the status and result URLs to the operation state.
            return operation_state
    
        except Exception as e:
            handle_exception(e)

@router.get(
    '/async-completions/{operation_id}/status',
    summary = 'Retrieve the status of the completion request operation with the specified operation ID.',
    responses = {
        200: {'description': 'The operation has completed.'},
        404: {'description': 'The operation was not found.'}
    }
)
async def get_operation_status(
    raw_request: Request,
    instance_id: str,
    operation_id: str,
    x_user_identity: Optional[str] = Header(None)
) -> OperationState:
    with tracer.start_as_current_span(f'get_operation_status') as span:
        try:
            span.set_attribute('operation_id', operation_id)
            span.set_attribute('instance_id', instance_id)
            
            background_response = await OrchestrationManager.get_operation_state(
                operation_id,
                raw_request.app.extra['config'],
                x_user_identity
            )
            print('BACKGROUND RESPONSE:', background_response)

            if background_response is None:
                raise HTTPException(status_code=404)

            if not background_response.completed:
                return Response(status_code=204)

            return background_response.response
        except Exception as e:
            handle_exception(e)

@router.get(
    '/async-completions/{operation_id}/result',
    summary = 'Retrieve the completion result of the operation with the specified operation ID.',
    responses = {
        200: {'description': 'The operation has completed.'},
        404: {'description': 'The operation was not found.'}
    }
)
async def get_operation_result(
    raw_request: Request,
    instance_id: str,
    operation_id: str,
    x_user_identity: Optional[str] = Header(None)
) -> CompletionResponse:
    with tracer.start_as_current_span(f'get_operatin_result') as span:
        try:
            span.set_attribute('operation_id', operation_id)
            span.set_attribute('instance_id', instance_id)
            
            background_response = await OrchestrationManager.get_operation_result(
                operation_id,
                raw_request.app.extra['config'],
                x_user_identity
            )

            if background_response is None:
                raise HTTPException(status_code=404)

            if not background_response.completed:
                return Response(status_code=204)

            return background_response.response
        except Exception as e:
            handle_exception(e)

async def create_completion_response(
    operation_id: str,
    instance_id: str,
    completion_request: KnowledgeManagementCompletionRequest,
    configuration: Configuration,
    x_user_identity: Optional[str] = Header(None)
):
    with tracer.start_as_current_span(f'create_completion_response') as span:
        try:
            span.set_attribute('operation_id', operation_id)
            span.set_attribute('instance_id', instance_id)
            span.set_attribute('request_id', completion_request.request_id)
            span.set_attribute('user_identity', x_user_identity)
            
            orchestration_manager = OrchestrationManager(
                completion_request = completion_request,
                configuration = configuration,
                context = Context(user_identity=x_user_identity)
            )

            completion = await orchestration_manager.ainvoke(completion_request)

            # TODO: This needs to send the completion to the state API and update the result of the operation object.
        

            #background_responses[operation_id].response = completion
            #background_responses[operation_id].completed = True

            print('COMPLETION RESPONSE:', completion)
        
        except Exception as e:
            #background_responses[operation_id].response = CompletionResponse(
            #    user_prompt = completion_request.user_prompt,
            #    completion = 'An error occurred in the LangChain orchestration service while processing the completion request.'
            #)
            #background_responses[operation_id].completed = True

            print('COMPLETION ERROR:', e)
