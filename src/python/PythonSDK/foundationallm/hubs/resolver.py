from abc import ABC, abstractmethod
from .metadata import Metadata
from .repository import Repository
from typing import List
from foundationallm.context import Context

class Resolver(ABC):
    """The Resolver class is responsible for resolving a request to a list of metadata value."""
    
    def __init__(self, repository: Repository):
        self.repository = repository

    @abstractmethod
    def resolve(self, request, user_context:Context=None, hint:str=None) -> List[Metadata]:
        pass