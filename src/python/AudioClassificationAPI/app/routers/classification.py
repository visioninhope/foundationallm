"""
The API endpoint for returning the completion from the LLM for the specified user prompt.
"""
from typing import Optional
from fastapi import APIRouter, Depends, Header, Request, Body
#from foundationallm.config import Context
#from foundationallm.telemetry import Telemetry
from app.dependencies import handle_exception#, validate_api_key_header
from app.classification import AudioClassifier
from foundationallm.models.classification import AudioPredictionRequest, AudioPredictionResponse

## Initialize telemetry logging
#logger = Telemetry.get_logger(__name__)
#tracer = Telemetry.get_tracer(__name__)

# Initialize API routing
router = APIRouter(
    prefix='/classification',
    tags=['classification'],
    #dependencies=[Depends(validate_api_key_header)],
    responses={404: {'description':'Not found'}}
)

@router.post('/predict')
async def get_prediction(
    request : Request,
    prediction_request: AudioPredictionRequest) -> AudioPredictionResponse:
    """
    Generates a prediction for the audio embeddings provided using a binary classification model.
    Predicts if the audio embeddings are of a Barred Forest-Falcon or an unknown bird species.
    
    Parameters
    ----------
    request : Request
        The underlying HTTP request.
    prediction_request : AudioPredictionRequest
        The request object containing the audio embeddings to classify.
    
    Returns
    -------
    AudioPredictionResponse
        Object containing the classification prediction.
    """
    #with tracer.start_as_current_span('completion') as span:
    #    try:
    #        span.set_attribute('request_id', completion_request.request_id)
    #        span.set_attribute('user_identity', x_user_identity)
    try:
        # orchestration_manager = OrchestrationManager(
        #     completion_request = completion_request,
        #     configuration=request.app.extra['config'],
        #     context=Context(user_identity=x_user_identity)
        # )
        # return orchestration_manager.invoke(completion_request)
        classifier = AudioClassifier(similarity_threshold=29, configuration=request.app.extra['config'])
        predicted_species, score = classifier.predict(prediction_request.embeddings)

        """
        Sample files for testing:
        {
          "file": "barred_forest_falcon_3920.wav",
          "similarity_threshold": 29
        },
        {
          "file": "dusky_capped_flycatcher_6513.wav",
          "similarity_threshold": 29
        },
            {
          "file": "barred_forest_falcon_19955.wav",
          "similarity_threshold": 29
        },
        {
          "file": "yellow_rumped_cacique_3195.wav",
          "similarity_threshold": 29
        },
        {
          "file": "barred_forest_falcon_10189.wav",
          "similarity_threshold": 29
        },
        {
          "file": "gray_fronted_dove_2823.wav",
          "similarity_threshold": 29
        }
    
        gray_fronted_dove_2688.wav is only background noise
        """

        if (predicted_species == 'barred_forest_falcon'):
            return AudioPredictionResponse(
                predicted_species_id='barred_forest_falcon',
                common_name='Barred Forest-Falcon',
                scientific_name='Micrastur ruficollis',
                species_ebird_code='baffal1',
                label='Sound of the call of a Barred Forest-Falcon (Micrastur ruficollis).',
                score=score
            )
        else:
            return AudioPredictionResponse(
                predicted_species_id='unknown',
                common_name='Unknown',
                scientific_name='Unknown',
                species_ebird_code='unknown',
                label='Sound of an unknown bird species.',
                score=score
            )
    except Exception as e:
        handle_exception(e)
