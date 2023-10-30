from abc import ABC, abstractmethod
from .metadata import Metadata
from .repository import Repository
from typing import List
from foundationallm.context import Context
from foundationallm.config import Configuration

class Resolver(ABC):
    """The Resolver class is responsible for resolving a request to a list of metadata value."""
    
    def __init__(self, repository: Repository, config:Configuration=None):
        self.repository = repository        
        self.config = config

    @abstractmethod
    def resolve(self, request, user_context:Context=None, hint:str=None) -> List[Metadata]:
        pass