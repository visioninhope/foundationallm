# Deployment - Standard with AKS

Compared to the quick start deployment using Azure Container Apps (ACA), the Foundationa**LLM** Standard Deployment with AKS is tailored for scaling up to production environments. It leverages Azure Kubernetes Service (AKS) for robust scalability and management, requiring an Azure Subscription with Azure OpenAI access.

Be mindful of the [Azure OpenaAI regional quota limits](https://learn.microsoft.com/en-us/azure/ai-services/openai/quotas-limits) on the number of Azure OpenAI Service instances.

This deployment option for FoundationaLLM uses Azure Kubernetes Service (AKS) to host the applications.  Compared to Azure Container Apps (ACA), AKS provides more advanced orchestration and scaling capabilities suitable for larger workloads. The Standard Deployment will configure OpenAI instances to use the maximum quota available.  If existing OpenAI resources are already deployed in the subscription, the Standard Deployment will not be able to deploy.  The Standard Deployment should be deployed to a subscription with no existing OpenAI resources, or a new subscription should be created for the Standard Deployment. As a final option, the template can be updated to allocate a smaller quota.

## Prerequisites

You will need the following resources and access to deploy the solution:

- Azure Subscription: An Azure Subscription is a logical container in Microsoft Azure that links to an Azure account and is the basis for billing, resource management, and allocation. It allows users to create and manage Azure resources like virtual machines, databases, and more, providing a way to organize access and costs associated with these resources.
- Subscription access to Azure OpenAI service: Access to Azure OpenAI Service provides users with the ability to integrate OpenAI's advanced AI models and capabilities within Azure. This service combines OpenAI's powerful models with Azure's robust cloud infrastructure and security, offering scalable AI solutions for a variety of applications like natural language processing and generative tasks. **Start here to [Request Access to Azure OpenAI Service](https://customervoice.microsoft.com/Pages/ResponsePage.aspx?id=v4j5cvGGr0GRqy180BHbR7en2Ais5pxKtso_Pz4b1_xUNTZBNzRKNlVQSFhZMU9aV09EVzYxWFdORCQlQCN0PWcu)**
- Minimum quota of 65 CPUs across all VM family types: Azure CPU quotas refer to the limits set on the number and type of virtual CPUs that can be used in an Azure Subscription. These quotas are in place to manage resource allocation and ensure fair usage across different users and services. Users can request quota increases if their application or workload requires more CPU resources. **Start here to [Manage VM Quotas](https://learn.microsoft.com/azure/quotas/per-vm-quota-requests)**
- App Registrations created in the Entra ID tenant (formerly Azure Active Directory): Azure App Registrations is a feature in Entra ID that allows developers to register their applications for identity and access management. This registration process enables applications to authenticate users, request and receive tokens, and access Azure resources that are secured by Entra ID. **Follow the instructions in the [Authentication setup document](authentication/index.md) to configure authentication for the solution.**
- User with the proper role assignments: Azure Role-Based Access Control (RBAC) roles are a set of permissions in Azure that control access to Azure resource management. These roles can be assigned to users, groups, and services in Azure, allowing granular control over who can perform what actions within a specific scope, such as a subscription, resource group, or individual resource.
    - Owner on the target subscription
    - Owner on the app registrations described in the Authentication setup document

You will use the following tools during deployment:

- Azure CLI ([v2.51.0 or greater](https://docs.microsoft.com/cli/azure/install-azure-cli)): The Azure Command Line Interface (CLI) is a set of commands used to manage Azure resources directly from the command line. It provides a simple and efficient way to automate tasks and manage Azure services, supporting both Windows, macOS, and Linux platforms. Azure CLI is particularly useful for scripting and executing batch operations, offering a comprehensive set of commands that cover almost all aspects of Azure resource management.
- [git](https://git-scm.com/downloads): Git is a distributed version control system designed to handle everything from small to very large projects with speed and efficiency. It allows multiple developers to work on the same codebase simultaneously, tracking and merging changes, and maintaining a complete history of all file revisions. Git is essential for modern software development, supporting branching and merging strategies, and is widely used for its robustness, flexibility, and remote collaboration capabilities.
- PowerShell 7 ([7.4.1 or greater](https://learn.microsoft.com/en-us/powershell/scripting/install/installing-powershell-on-windows?view=powershell-7.4)):
PowerShell 7 is a cross-platform (Windows, macOS, and Linux) automation tool and scripting language, an evolution of PowerShell that works with the .NET Core framework. It offers enhanced features and performance improvements over its predecessors and is designed for heterogeneous environments and the hybrid cloud. In PowerShell 7, the command-line executable is referred to as pwsh, an alias that is essential for integration with Azure Developer CLI (AZD) hooks and other modern automation scenarios.
- [Helm](https://helm.sh/docs/intro/install/) is a package manager for Kubernetes, an open-source platform for automating the deployment, scaling, and operations of application containers across clusters of hosts. It simplifies the process of defining, installing, and upgrading even the most complex Kubernetes applications. Helm works by bundling related Kubernetes resources into a single unit called a chart, which can be easily shared, updated, and deployed.
- [`kubectl`](https://kubernetes.io/docs/tasks/tools/) is a command-line tool that allows users to run commands against Kubernetes clusters, providing essential functionality to manage applications and resources within a Kubernetes environment. It enables users to deploy applications, inspect and manage cluster resources, and view logs. `kubectl` serves as the primary interface for interacting with Kubernetes, offering a wide range of capabilities for controlling and automating different aspects of the cluster and its workloads.

**Optional** To run or debug the solution locally, you will need to install the following dependencies:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0): The .NET 8 SDK is the latest iteration of Microsoft's .NET software development kit, offering enhanced features and improvements for building applications across various platforms, including web, mobile, desktop, cloud, and IoT. It provides a comprehensive set of libraries, runtime features, and APIs, supporting multiple programming languages like C#, F#, and Visual Basic. This SDK is designed for high performance and efficiency, catering to modern development needs with support for cloud-native applications, microservices, and machine learning.
- Visual Studio 2022: Visual Studio 2022 is an advanced integrated development environment (IDE) from Microsoft, offering robust tools for developing applications on the .NET platform and other technologies. It brings improved performance, better usability, and enhanced collaboration features, supporting a wide range of programming languages and frameworks. Visual Studio 2022 is tailored for both individual developers and teams, integrating seamlessly with modern workflows and cloud services, and providing powerful debugging, code navigation, and refactoring capabilities.

**Optional** To build or test container images, you will need to install the following dependencies:

- [Docker Desktop](https://www.docker.com/products/docker-desktop/): Docker Desktop is an application for MacOS and Windows machines for the building and sharing of containerized applications and microservices. It provides an integrated environment to use Docker containers, simplifying the process of building, testing, and deploying applications in a consistent and isolated environment. Docker Desktop includes Docker Engine, Docker CLI client, Docker Compose, and other Docker tools, making it a key tool for developers working with container-based applications.

## Pre-Deployment steps

Follow the steps below to deploy the solution to your Azure subscription.

1. Ensure all the prerequisites are met.

2. From a PowerShell prompt, execute the following to clone the repository:

    ```powershell
    git clone https://github.com/solliancenet/foundationallm.git
    cd foundationallm
    git checkout release/0.5.0
    ```
3. Create your deployment manifest:

    ```powershell
    cd deploy/standard
    cp Deployment-Manifest.template.json Deployment-Manifest.json
    ```

4. Fill out all required fields in the `Deployment-Manifest.json` file. Please look at [this guide](./standard/manifest.md) for more information on the manifest contents.

5. Login to your Azure account and set the deployment subscription:

    ```powershell
    az login
    az account set --subscription <Azure Subscription ID>
    ```

6. **Optional** Execute the pre-deployment script:

    ```powershell
    cd scripts
    ./Pre-Deploy.ps1
    ```

    > [!NOTE]
    > The pre-deployment script will acquire certificates from LetsEncrypt and place them in `foundationallm/deploy/standard/config/certbot/certs`.  If you plan to provide certificates another way, you can skip this script, but you must place the certificates in the specified location.  See step 7.

7. **Optional** Provide certificates (manual method):

    > [!NOTE]
    > Skip this step if you created certificates using LetsEncrypt in step 6.

    Create certificates for the appropriate domains and package them in PFX format.  Place the PFX files in `foundationallm/deploy/standard/config/certbot/certs` following the naming convention below.  The values for `Host Name` and `Domain Name` should match the values you provided in your deployment manifest:

    | Service Name | Host Name | Domain Name | File Name |
    | -- | -- | -- | -- |
    | core-api | api | example.com | api.example.com.pfx | 
    | management-api | management-api | example.com | management-api.example.com.pfx |
    | vectorization-api | vectorization-api | example.com | vectorization-api.example.com.pfx |
    | chat-ui | chat | example.com | chat.example.com.pfx |
    | management-ui | management | example.com | management.example.com.pfx |
----


4. **Navigate to the bicep deployment directory:**
   - Execute the following command to navigate:
     ```powershell
     cd foundationallm/deploy/standard/bicep
     ```

5. **Load psake into your PowerShell session:**
   - Execute the following command:
     ```powershell
     ./bootstrap.ps1
     ```

6. **Execute the deployment:**
   - Execute the following command to deploy the solution:
     ```powershell
     Invoke-psake
     ```

### Additional Notes:
- **Azure OpenAI Service Regions:**
  - Ensure that the `<location>` value in your deployment script corresponds to a region that supports Azure OpenAI services. Refer to Azure OpenAI service regions for more information.

## Post-deployment configuration

### Authentication setup

Follow the instructions in the [Authentication setup document](https://docs.foundationallm.ai/deployment/authentication/index.html) to configure authentication for the solution.

## References

### Deployment-Manifest.json Reference

| Property         | Description                                                                                      | Default Value     |
| ---------------- | ------------------------------------------------------------------------------------------------ | ----------------- |
| environment      | Target deployment environment                                                                    | `stg`             |
| location         | Target deployment region in Azure                                                                | `EastUS2`         |
| project          | Project identifier associated with this deployment                                               | `fllm01`          |
| subscriptoin     | Target deployment subscription in Azure                                                          | N/A               |
| k8sNamespace     | Target Kubernetes namespace for deployed services in AKS clusters                                | `default`         |
| adminObjectId    | Object Id of the Azure AD User or Group to grant appropriate rights to administer the deployment | N/A               |
| createVpnGateway | Flag to enable/disable creation of a VPN Gateway for private network access                      | `true`            |
| publicEndpoints  | Flag to enable/disable exposing service endpoints on the public Internet                         | `true`            |
| createApimUdr    | Flag to enable/disable creating UDR rule to support APIM deployment in a peered VNET environment | `false`           |
| vnetName         | Desired VNET name for deployment.                                                                | N/A               |
| vnetCidr         | Desired VNET CIDR Address range.                                                                 | `10.220.128.0/21` |
