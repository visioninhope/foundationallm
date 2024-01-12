# Role management

FoundationaLLM roles are assigned to users, groups, service principals, and managed identities through the Management API. As described in the [role definitions](role-definitions.md) article, administrators can apply fine-grained access control to features and resources to ensure the deployment adheres to least-privilege best practices when properly assigned.

## Role management endpoints

The Management API provides the following role management endpoints:

| Method | Endpoint | Description |
| --- | --- | --- |
| GET | `/security/roles` | Returns a list of all roles. |
| GET | `/security/roles/{RoleDefinitionId}` | Returns the role settings for the specified role. |
| GET | `/security/roles/{RoleDefinitionId}/assignments` | Returns a list of all role assignments for the specified role. |
| GET | `/security/roles/{RoleDefinitionId}/assignments/{RoleAssignmentId}` | Returns the role assignment settings for the specified role assignment. |
| GET | `/security/roles/{Scope}` | Returns a list of all roles at the specified scope. |
| GET | `/security/roles/{Scope}/assignments` | Returns a list of all role assignments at the specified scope. |
| POST | `/security/roles/assign` | Assigns a role to an Entra ID user or group. |
| POST | `/security/roles/revoke` | Revokes a role from an Entra ID user or group. |

> The Management Portal provides a graphical user interface over the Management API for managing roles and role assignments, among other FLLM configuration settings.
