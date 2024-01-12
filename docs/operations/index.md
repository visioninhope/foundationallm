### Standard AKS: Platform Administration Documentation

1. **Environment Setup:**
   - [Initial Environment Setup](https://github.com/solliancenet/foundationallm/blob/mg-terraform-iac/deploy/standard/bicep/README.md)
   - Configuration settings for different components:
     - [App Configuration Settings](../deployment/app-configuration-values.md)
     - [Configuration for deployment](../deployment/deployment-configuration.md)
     - [Configure Core API Settings](../deployment/authentication/core-authentication-setup-entra.md#update-app-configuration-settings)
     - [Configure Management API Settings](../deployment/authentication/management-authentication-setup-entra.md#update-app-configuration-settings)

2. **User Management:**
   - Creating, modifying, and deleting user accounts:
     - [Authentication setup](../deployment/authentication/index.md)
   - Assigning roles and permissions:
     - [Access control for azure services](../deployment/configure-access-control-for-services.md)
     - FoundationaLLM roles and permissions

3. **System Maintenance:**
   - [FoundationaLLM Backups & Data Resiliency](./Backups.md)

4. **Security Measures:**
   - [Platform Security Features & Best Practices](./Security.md)

5. **Logging and Auditing:**
   - [Accessing System Logs & Audit Trails](./Logs.md)

6. **Troubleshooting:**
   - [Troubleshooting & Issue Reporting Guide](./Troubleshooting.md)

7. **Platform Features:**
   - [Detailed documentation on each feature of the platform](https://docs.foundationallm.ai/)
   - TODO - Use cases and best practices for utilizing specific features.

### Update Process:

1. **Release Notes:**
   - [Guidance for creating release notes](./Release-Notes.md)

2. **Update Procedure:**
   - [Updating container deployments](./Update.md)

3. **Vulnerabilities:**
   - [Vulerabilities: Identification, Communication, and Remediation](./Vulnerabilities.md)
