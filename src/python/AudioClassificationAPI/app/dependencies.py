"""
Provides dependencies for API calls.
"""
import logging
import time
from fastapi import Depends, HTTPException
from fastapi.security import APIKeyHeader
from foundationallm.config import Configuration

__config: Configuration = None
API_NAME = 'AudioClassificationAPI'

def get_config(action: str = None) -> Configuration:
    """
    Obtains the application configuration settings.
    
    Returns
    -------
    Configuration
        Returns the application configuration settings.
    """
    global __config

    start = time.time()
    if action is not None and action=='refresh':
        __config = Configuration()
    else:
        __config = __config or Configuration()
    end = time.time()
    print(f'Time to load config: {end-start}')
    return __config

def handle_exception(exception: Exception, status_code: int = 500):
    """
    Handles an exception that occurred while processing a request.
    
    Parameters
    ----------
    exception : Exception
        The exception that occurred.
    """
    #logging.error(exception, stack_info=True, exc_info=True)
    raise HTTPException(
        status_code = status_code,
        detail = str(exception)
    ) from exception
