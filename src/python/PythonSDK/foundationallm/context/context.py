import json
from foundationallm.context import UserIdentity

class Context:
    def __init__(self, user_identity: UserIdentity = None):
        """Init"""
        self.user_identity = json.loads(user_identity, object_hook=UserIdentity.from_json) or None
        
    def get_upn(self) -> str:
        return self.user_identity.upn or ''