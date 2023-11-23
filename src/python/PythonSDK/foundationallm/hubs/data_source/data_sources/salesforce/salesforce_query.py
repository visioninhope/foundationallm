from pydantic import BaseModel

class SalesforceQuery(BaseModel):
    name : str
    description : str
    query : str
