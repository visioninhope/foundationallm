using FoundationaLLM.Authorization.Constants;
using System.Collections.ObjectModel;

namespace FoundationaLLM.Authorization.Models
{
    /// <summary>
    /// Defines all authorizable actions managed by the FoundationaLLM.Authorization resource provider.
    /// </summary>
    public static class AuthorizableActions
    {
        public static readonly ReadOnlyDictionary<string, AuthorizableAction> Actions = new(
            new Dictionary<string, AuthorizableAction>()
            {
                {
                    AuthorizableActionNames.FoundationaLLM_Authorization_RoleAssignments_Read,
                    new AuthorizableAction(
                        AuthorizableActionNames.FoundationaLLM_Authorization_RoleAssignments_Read,
                        "Read role assignments.",
                        "Authorization")
                },
                {
                    AuthorizableActionNames.FoundationaLLM_Authorization_RoleAssignments_Write,
                    new AuthorizableAction(
                        AuthorizableActionNames.FoundationaLLM_Authorization_RoleAssignments_Write,
                        "Create or update role assignments.",
                        "Authorization")
                },
                {
                    AuthorizableActionNames.FoundationaLLM_Authorization_RoleAssignments_Delete,
                    new AuthorizableAction(
                        AuthorizableActionNames.FoundationaLLM_Authorization_RoleAssignments_Delete,
                        "Delete role assignments.",
                        "Authorization")
                },
            });
    }
}