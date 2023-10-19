from typing import Optional
from langchain.tools import BaseSQLDatabaseTool
from langchain.tools.base import BaseTool
from langchain.pydantic_v1 import Field
from langchain.callbacks.manager import (
    CallbackManagerForToolRun
)

class SecureSQLDatabaseQueryTool(BaseSQLDatabaseTool, BaseTool):
    """Tool for conditionally adding row-level security to a SQL query."""

    name: str = "secure_sql_db_query"
    description: str = (
        "Input to this tool is a detailed and correct SQL query, output is a result from the database. "
        "If the query is not correct, an error message will be returned. If an error is returned, rewrite the query, check the query, and try again. "
        "If you encounter an issue with Unknown column 'xxxx' in 'field list', using sql_db_schema to query the correct table fields."
    )

    username: str = Field(exclude=True)
    apply_row_level_security: bool = Field(exclude=True)

    def _run(
        self,
        query: str,
        run_manager: Optional[CallbackManagerForToolRun] = None,
    ) -> str:
        """Conditionally, wrap the query with EXECUTE AS USER and REVERT statements."""
        if (self.apply_row_level_security) and (self.db.dialect=='mssql'):
            query = (
                f"EXECUTE AS USER = '{self.username}'; "
                f"{query}; "
                "REVERT;"
            )
        else:
            query = query
        
        return self.db.run_no_throw(query)