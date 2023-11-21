from foundationallm.hubs import Metadata

class SalesforceAuthenticationMetadata(Metadata):
    client_id: str
    client_secret: str
    refresh_token: str
    instance_url: str