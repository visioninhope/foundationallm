# Core Examples

This directory contains a collection of examples that demonstrate how to use the FoundationaLLM Core API. Each example is a standalone test that demonstrates a specific feature or capability of the Core API. You can run these examples using the Visual Studio Test Explorer, the command line, or by right-clicking on the test file and selecting **Run Tests**.

## Prerequisites

> [!IMPORTANT]
> Please ensure that you have [set up your local development environment](../../../docs/development/development-local.md#prerequisites) prerequisites.

1. Copy the `testsettings.example.json` file to `testsettings.json` and update the values based on your requirements.
2. Make sure you are signed in to Visual Studio with your account you use to access the Azure subscription to which your FoundationaLLM solution is deployed.
3. Log in to Azure via the Azure CLI using the command `az login`.

## Examples

### Example 1: FoundationaLLM agent interaction

**Purpose**: Verify that the `FoundationaLLM` agent is available and can respond to user queries.

**File**: [Example0001_FoundationaLLMAgentInteraction.cs](Example0001_FoundationaLLMAgentInteraction.cs)

This example demonstrates how to use the FoundationaLLM Core API to send user prompts to the default `FoundationaLLM` agent and receive completions. It sends both session-based and sessionless requests to the Core API.

#### Running the example

Run the example by running a test on the `Example0001_FoundationaLLMAgentInteraction.cs` file. You can run the test using the Visual Studio Test Explorer, the command line, or by simply right-clicking anywhere on the `Example0001_FoundationaLLMAgentInteraction.cs` file and selecting **Run Tests**.

![The Run Tests context menu option is displayed.](media/example-1-run-tests.png)

You will see an output similar to the following after the test is completed:

![The completed test is displayed.](media/example-1-completed-test.png)


### Example 4: Synchronous vectorization of a file located in Azure Data Lake Storage Gen2

**Purpose**: Run synchronous vectorization of a file located in Azure Data Lake Storage Gen2.

**File**: [Example0004_SynchronousVectorizationOfPDFFromDataLake.cs](Example0004_SynchronousVectorizationOfPDFFromDataLake.cs)

This example demonstrates a synchronous vectorization request for a file located in a storage account.

#### Setup

This example expects the following file named [`SDZWA-Journal-January-2024.pdf`](https://sandiegozoowildlifealliance.org/Journal/january-2024) to be located `vectorization-input` container in the data lake storage account created with the FoundationaLLM deployment.

##### App Config settings
| Key | Value | Description |
| --- | --- | --- |
| `FoundationaLLM:DataSources:datalake_vectorization_input:AuthenticationType` | `AzureIdentity` | The authentication method for the vectorization api and vectorization job managed identities. |
| `FoundationaLLM:DataSources:datalake_vectorization_input:AccountName` | N/A | Account name of the storage account. |

#### Running the example

Run the example by running a test on the `Example0004_SynchronousVectorizationOfPDFFromDataLake.cs` file. You can run the test using the Visual Studio Test Explorer, the command line, or by simply right-clicking anywhere on the `Example0004_SynchronousVectorizationOfPDFFromDataLake.cs` file and selecting **Run Tests**.

You will see an output similar to the following after the test is completed:

```text
============ Synchronous Vectorization of a PDF from Data Lake ============
Create the data source: datalake_vectorization_input via the Management API
Create the vectorization text partitioning profile: text_partition_profile via the Management API
Create the vectorization text embedding profile: text_embedding_profile_generic via the Management API
Create the vectorization indexing profile: indexing_profile_pdf via the Management API
Create the vectorization request: ab74c501-6e49-41ac-95bf-7284174564c8 via the Management API
Verify the vectorization request ab74c501-6e49-41ac-95bf-7284174564c8 was created by retrieving it from the Management API
Issue the process action on the vectorization request: ab74c501-6e49-41ac-95bf-7284174564c8 via the Management API
Vectorization request: ab74c501-6e49-41ac-95bf-7284174564c8 completed successfully.
Verify a search yields 27 documents.
Delete the data source: datalake_vectorization_input via the Management API
Delete the vectorization text partitioning profile: text_partition_profile via the Management API
Delete the vectorization text embedding profile: text_embedding_profile_generic via the Management API
Delete the vectorization indexing profile: indexing_profile_pdf via the Management API and delete the created index
```

### Example 5: Asynchronous vectorization of a file located in Azure Data Lake Storage Gen2

**Purpose**: Run synchronous vectorization of a file located in Azure Data Lake Storage Gen2.

**File**: [Example0005_AsynchronousVectorizationOfPDFFromDataLake.cs](Example0005_AsynchronousVectorizationOfPDFFromDataLake.cs)

This example demonstrates a synchronous vectorization request for a file located in a storage account.

#### Setup

This example expects the following file named [`SDZWA-Journal-January-2024.pdf`](https://sandiegozoowildlifealliance.org/Journal/january-2024) to be located `vectorization-input` container in the data lake storage account created with the FoundationaLLM deployment.

##### App Config settings
| Key | Value | Description |
| --- | --- | --- |
| `FoundationaLLM:DataSources:datalake_vectorization_input:AuthenticationType` | `AzureIdentity` | The authentication method for the vectorization api and vectorization job managed identities. |
| `FoundationaLLM:DataSources:datalake_vectorization_input:AccountName` | N/A | Account name of the storage account. |

#### Running the example

Run the example by running a test on the `Example0005_AsynchronousVectorizationOfPDFFromDataLake.cs` file. You can run the test using the Visual Studio Test Explorer, the command line, or by simply right-clicking anywhere on the `Example0005_AsynchronousVectorizationOfPDFFromDataLake.cs` file and selecting **Run Tests**.

You will see an output similar to the following after the test is completed:

```text
============ Asynchronous Vectorization of a PDF from Data Lake ============
Create the data source: datalake_vectorization_input via the Management API
Create the vectorization text partitioning profile: text_partition_profile via the Management API
Create the vectorization text embedding profile: text_embedding_profile_generic via the Management API
Create the vectorization indexing profile: indexing_profile_pdf via the Management API
Create the vectorization request: 912e93ba-f5cb-4398-9ab6-c13f986269a2 via the Management API
Verify the vectorization request 912e93ba-f5cb-4398-9ab6-c13f986269a2 was created by retrieving it from the Management API
Issue the process action on the vectorization request: 912e93ba-f5cb-4398-9ab6-c13f986269a2 via the Management API
Get the initial processing state for the vectorization request: 912e93ba-f5cb-4398-9ab6-c13f986269a2 via the Management API
Polling the processing state of the async vectorization request: 912e93ba-f5cb-4398-9ab6-c13f986269a2 by retrieving the request from the Management API
Vectorization request: 912e93ba-f5cb-4398-9ab6-c13f986269a2 completed successfully.
Verify a search yields 27 documents.
Delete the data source: datalake_vectorization_input via the Management API
Delete the vectorization text partitioning profile: text_partition_profile via the Management API
Delete the vectorization text embedding profile: text_embedding_profile_generic via the Management API
Delete the vectorization indexing profile: indexing_profile_pdf via the Management API along with the index
```

### Example 6: Synchronous vectorization of a file located in a OneLake Lakehouse

**Purpose**: Run synchronous vectorization of a file located in a OneLake Lakehouse.

**File**: [Example0006_SynchronousVectorizationOfPDFFromOneLake.cs](Example0006_SynchronousVectorizationOfPDFFromOneLake.cs)

This example demonstrates a synchronous vectorization request for a file located in a OneLake Lakehouse.

#### Setup

This example expects the following file named [`SDZWA-Journal-January-2024.pdf`](https://sandiegozoowildlifealliance.org/Journal/january-2024) to be located in the lakehouse files.

##### Permissions

The vectorization api and vectorization job managed identities need to have `Contributor` permissions on the workspace.

##### App Config settings
| Key | Value | Description |
| --- | --- | --- |
| `FoundationaLLM:DataSources:datalake_vectorization_input:AuthenticationType` | `AzureIdentity` | The authentication method for the vectorization api and vectorization job managed identities. This will always be `AzureIdentity`. |
| `FoundationaLLM:DataSources:datalake_vectorization_input:AccountName` | `onelake` | Account name - this will always be `onelake`. |

#### Running the example

Run the example by running a test on the `Example0006_SynchronousVectorizationOfPDFFromOneLake.cs` file. You can run the test using the Visual Studio Test Explorer, the command line, or by simply right-clicking anywhere on the `Example0006_SynchronousVectorizationOfPDFFromOneLake.cs` file and selecting **Run Tests**.

You will see an output similar to the following after the test is completed:

```text
============ Synchronous Vectorization of a PDF from OneLake ============
Create the data source: onelake_fllm via the Management API
Create the vectorization text partitioning profile: text_partition_profile via the Management API
Create the vectorization text embedding profile: text_embedding_profile_generic via the Management API
Create the vectorization indexing profile: indexing_profile_pdf via the Management API
Create the vectorization request: f9eddadd-82a8-4068-8956-8669e0c1b020 via the Management API
Verify the vectorization request f9eddadd-82a8-4068-8956-8669e0c1b020 was created by retrieving it from the Management API
Issue the process action on the vectorization request: f9eddadd-82a8-4068-8956-8669e0c1b020 via the Management API
Vectorization request: f9eddadd-82a8-4068-8956-8669e0c1b020 completed successfully.
Verify a search yields 27 documents.
Delete the data source: onelake_fllm via the Management API
Delete the vectorization text partitioning profile: text_partition_profile via the Management API
Delete the vectorization text embedding profile: text_embedding_profile_generic via the Management API
Delete the vectorization indexing profile: indexing_profile_pdf via the Management API and delete the created index
```

### Example 8: Synchronous vectorization of a file located in SharePoint Online

**Purpose**: Run synchronous vectorization of a file located SharePoint Online.

**File**: [Example0008_SynchronousVectorizationOfPDFFromSharePoint.cs](Example0008_SynchronousVectorizationOfPDFFromSharePoint.cs)

This example demonstrates a synchronous vectorization request for a file located in SharePoint Online.

#### Setup

This example expects a service principal to be created using the following guidance: [Create a service principal with access to SharePoint Online](https://learn.microsoft.com/en-us/azure/data-factory/connector-sharepoint-online-list?tabs=data-factory#prerequisites) REFER TO THE **Prerequisites** SECTION ONLY.

The certificate used to authenticate the service principal needs to be uploaded to the Azure Key Vault.

This example expects the following file named [`SDZWA-Journal-January-2024.pdf`](https://sandiegozoowildlifealliance.org/Journal/january-2024) to be located in a SharePoint Online site.

##### App Config settings
| Key | Value | Description |
| --- | --- | --- |
| `FoundationaLLM:DataSources:sharepoint_fllm:ClientId` | N/A | The ClientId of the service principal accessing SharePoint Online. |
| `FoundationaLLM:DataSources:sharepoint_fllm:TenantId` | N/A | The TenantId of the serivce principal accessing SharePoint Online. |
| `FoundationaLLM:DataSources:sharepoint_fllm:CertificateName` | N/A | The name of the certificate in the Azure Key Vault used to authenticate the service principal. |
| `FoundationaLLM:DataSources:sharepoint_fllm:KeyVaultURL` | N/A | The URL of the Azure Key Vault where the certificate is stored. |

##### `testsettings.json` settings

The test settings file provides information to the vectorization service about the location and file name of the document to vectorize. The following is an example of the `testsettings.json` file for this example:

```json
{
  "SharePointVectorizationConfiguration": {
	"HostName": "fllm.sharepoint.com",
	"SitePath": "sites/FoundationaLLM",
	"FolderPath": "SDZWA/Journals",
	"FileName": "SDZWA-Journal-January-2024.pdf"
  }
}
```

Property definitions:

- `HostName`: Host name of the SharePoint site without the protocol, ex: `fllm.sharepoint.com`.
- `SitePath`: The relative path of the site/subsite, ex: `/sites/FoundationaLLM`.
- `FolderPath`: The folder path, starting with the document library, ex: `SDZWA/Journals`.
- `FileName`: The file name of the document to vectorize.

#### Running the example

Run the example by running a test on the `Example0008_SynchronousVectorizationOfPDFFromSharePoint.cs` file. You can run the test using the Visual Studio Test Explorer, the command line, or by simply right-clicking anywhere on the `Example0008_SynchronousVectorizationOfPDFFromSharePoint.cs` file and selecting **Run Tests**.

You will see an output similar to the following after the test is completed:

```text
============ Synchronous Vectorization of a PDF from SharePoint Online ============
Create the data source: sharepoint_fllm via the Management API
Create the vectorization text partitioning profile: text_partition_profile via the Management API
Create the vectorization text embedding profile: text_embedding_profile_generic via the Management API
Create the vectorization indexing profile: indexing_profile_pdf via the Management API
Create the vectorization request: 01f507af-7780-4c5e-b8e8-198e7ea6fcb0 via the Management API
Verify the vectorization request 01f507af-7780-4c5e-b8e8-198e7ea6fcb0 was created by retrieving it from the Management API
Issue the process action on the vectorization request: 01f507af-7780-4c5e-b8e8-198e7ea6fcb0 via the Management API
Vectorization request: 01f507af-7780-4c5e-b8e8-198e7ea6fcb0 completed successfully.
Verify a search yields 27 documents.
Delete the data source: sharepoint_fllm via the Management API
Delete the vectorization text partitioning profile: text_partition_profile via the Management API
Delete the vectorization text embedding profile: text_embedding_profile_generic via the Management API
Delete the vectorization indexing profile: indexing_profile_pdf via the Management API along with the index
```

### Example 9: Asynchronous vectorization of a file located in SharePoint Online

**Purpose**: Run asynchronous vectorization of a file located SharePoint Online.

**File**: [Example0009_AsynchronousVectorizationOfPDFFromSharePoint.cs](Example0009_AsynchronousVectorizationOfPDFFromSharePoint.cs)

This example demonstrates an asynchronous vectorization request for a file located in SharePoint Online.

#### Setup

This example expects a service principal to be created using the following guidance: [Create a service principal with access to SharePoint Online](https://learn.microsoft.com/en-us/azure/data-factory/connector-sharepoint-online-list?tabs=data-factory#prerequisites) REFER TO THE **Prerequisites** SECTION ONLY.

The certificate used to authenticate the service principal needs to be uploaded to the Azure Key Vault.

This example expects the following file named [`SDZWA-Journal-January-2024.pdf`](https://sandiegozoowildlifealliance.org/Journal/january-2024) to be located in a SharePoint Online site.

##### App Config settings
| Key | Value | Description |
| --- | --- | --- |
| `FoundationaLLM:DataSources:sharepoint_fllm:ClientId` | N/A | The ClientId of the service principal accessing SharePoint Online. |
| `FoundationaLLM:DataSources:sharepoint_fllm:TenantId` | N/A | The TenantId of the serivce principal accessing SharePoint Online. |
| `FoundationaLLM:DataSources:sharepoint_fllm:CertificateName` | N/A | The name of the certificate in the Azure Key Vault used to authenticate the service principal. |
| `FoundationaLLM:DataSources:sharepoint_fllm:KeyVaultURL` | N/A | The URL of the Azure Key Vault where the certificate is stored. |

##### `testsettings.json` settings

The test settings file provides information to the vectorization service about the location and file name of the document to vectorize. The following is an example of the `testsettings.json` file for this example:

```json
{
  "SharePointVectorizationConfiguration": {
	"HostName": "fllm.sharepoint.com",
	"SitePath": "sites/FoundationaLLM",
	"FolderPath": "SDZWA/Journals",
	"FileName": "SDZWA-Journal-January-2024.pdf"
  }
}
```

Property definitions:

- `HostName`: Host name of the SharePoint site without the protocol, ex: `fllm.sharepoint.com`.
- `SitePath`: The relative path of the site/subsite, ex: `/sites/FoundationaLLM`.
- `FolderPath`: The folder path, starting with the document library, ex: `SDZWA/Journals`.
- `FileName`: The file name of the document to vectorize.

#### Running the example

Run the example by running a test on the `Example0009_AsynchronousVectorizationOfPDFFromSharePoint.cs` file. You can run the test using the Visual Studio Test Explorer, the command line, or by simply right-clicking anywhere on the `Example0009_AsynchronousVectorizationOfPDFFromSharePoint.cs` file and selecting **Run Tests**.

You will see an output similar to the following after the test is completed:

```text
============ Asynchronous Vectorization of a PDF from SharePoint Online ============
Create the data source: sharepoint_fllm via the Management API
Create the vectorization text partitioning profile: text_partition_profile via the Management API
Create the vectorization text embedding profile: text_embedding_profile_generic via the Management API
Create the vectorization indexing profile: indexing_profile_pdf via the Management API
Create the vectorization request: 2b88e6d2-a51c-4e38-b5d7-e1a7f72cb694 via the Management API
Verify the vectorization request 2b88e6d2-a51c-4e38-b5d7-e1a7f72cb694 was created by retrieving it from the Management API
Issue the process action on the vectorization request: 2b88e6d2-a51c-4e38-b5d7-e1a7f72cb694 via the Management API
Get the initial processing state for the vectorization request: 2b88e6d2-a51c-4e38-b5d7-e1a7f72cb694 via the Management API
Polling the processing state of the async vectorization request: 2b88e6d2-a51c-4e38-b5d7-e1a7f72cb694 by retrieving the request from the Management API
Vectorization request: 2b88e6d2-a51c-4e38-b5d7-e1a7f72cb694 completed successfully.
Verify a search yields 27 documents.
Delete the data source: sharepoint_fllm via the Management API
Delete the vectorization text partitioning profile: text_partition_profile via the Management API
Delete the vectorization text embedding profile: text_embedding_profile_generic via the Management API
Delete the vectorization indexing profile: indexing_profile_pdf via the Management API and delete the created index
```


### Example 16: Completion quality measurements with Azure AI Studio

**Purpose**: Verify that the completion quality measurements can be completed successfully with Azure AI Studio.

**File**: [Example0016_CompletionQualityMeasurements.cs](Example0016_CompletionQualityMeasurements.cs)

This example demonstrates how to use the FoundationaLLM Core API to send predefined user prompts with their expected outcomes to evaluate completion quality measurements with Azure AI Studio.

#### Setup

##### App Config settings

This example requires adding Azure AI Studio and its related storage account settings to the deployed App Config service. Create the following settings in the App Config service:

| Key | Default Value | Description |
| --- | --- | --- |
| `FoundationaLLM:AzureAIStudio:BaseUrl` | `https://ai.azure.com` | The base URL of the Azure AI Studio API. |
| `FoundationaLLM:AzureAIStudio:ContainerName` | N/A | Container where Azure AI Studio stores the data sets. |
| `FoundationaLLM:AzureAIStudio:SubscriptionId` | N/A | Subscription ID associated with the Azure AI Studio deployment. |
| `FoundationaLLM:AzureAIStudio:Region` | N/A | Region of the Azure AI Studio deployment. |
| `FoundationaLLM:AzureAIStudio:ResourceGroup` | N/A | Resource Group of the Azure AI Studio deployment. |
| `FoundationaLLM:AzureAIStudio:ProjectName` | N/A | Project Name of the Azure AI Studio deployment. |
| `FoundationaLLM:AzureAIStudio:Deployment` | `gpt-35-turbo-16k` | Azure AI Studio GPT model deployment name. |
| `FoundationaLLM:AzureAIStudio:Metrics` | `gpt_groundedness,gpt_relevance,gpt_coherence,gpt_fluency,gpt_similarity` | Metrics to run on the Azure AI Studio. |
| `FoundationaLLM:AzureAIStudio:FlowDefinitionResourceId` | N/A | The Flow Definition Resource ID of the Azure AI Studio. |
| `FoundationaLLM:AzureAIStudio:BlobStorageServiceSettings:AuthenticationType` | `ConnectionString` | The method by which this example connects to the storage account associated with the Azure AI Studio deployment. Valid options are `ConnectionString`, `AccountKey`, and `AzureIdentity`. |
| `FoundationaLLM:AzureAIStudio:BlobStorageServiceSettings:AccountName` | N/A | The name of the storage account. |
| `FoundationaLLM:AzureAIStudio:BlobStorageServiceSettings:AccountKey` | N/A | The account key if you are using the `AccountKey` connection type. We strongly recommend making this a Key Vault reference. |
| `FoundationaLLM:AzureAIStudio:BlobStorageServiceSettings:ConnectionString` | N/A | The account connection string if you are using the `ConnectionString` connection type. We strongly recommend making this a Key Vault reference. |

##### `testsettings.json` settings

Add one or more user prompts with their expected outcomes to the `testsettings.json` file within the `CompletionQualityMeasurementConfiguration` section. The example will use these prompts to evaluate completion quality measurements with Azure AI Studio.

Example:

```json
{
  "CompletionQualityMeasurementConfiguration": {
    "AgentPrompts": [
      {
        "AgentName": "FoundationaLLM",
        "CreateAgent": false,
        "SessionConfiguration": {
          "Sessionless": false,
          "CreateNewSession": true,
          "SessionId": ""
        },
        "UserPrompt": "What are the top three features that FoundationaLLM provides when it comes to asking questions about private data sources?",
        "ExpectedCompletion": "FoundationaLLM simplifies integration with enterprise data sources used by agents for in-context learning, provides fine-grain security controls over data used by agents, and offers pre/post completion filters that guard against attack."
      }
    ]
  },
}
```

Property definitions:

- `AgentPrompts`: An array of user prompts with their expected outcomes.
  - `AgentName`: The name of the agent sent to the Core API completions endpoint.
  - `CreateAgent`: Indicates whether to create a new agent for the test run. If `true`, the agent will be created and deleted. If set to `true`, make sure you add the agent to the `Catalogs.AgentCatalog`. Default value is `false`.
  - `SessionConfiguration`: Controls the configuration of the chat session  .
    - `Sessionless`: If `true`, the chat session will not be stored in the database and the session ID will be ignored. Default value is `false`.
    - `CreateNewSession`: Create a new chat session rather than using an existing one. Default value is `true`.
    - `SessionId`: If you are not creating a new chat session, enter the existing session ID here. Default value is an empty string.
  - `UserPrompt`: The user prompt sent to the Core API completions endpoint.
  - `ExpectedCompletion`: Used for quality measurements. The expected completion for the user prompt.

#### Running the example

1. Run the example by running a test on the `Example16_CompletionQualityMeasurements.cs` file. You can run the test using the Visual Studio Test Explorer, the command line, or by simply right-clicking anywhere on the `Example16_CompletionQualityMeasurements.cs` file and selecting **Run Tests**.

    You will see an output similar to the following after the test is completed:

    ![The completed test is displayed.](media/example-16-completed-test.png)

2. The test will send the user prompt to the Core API completions endpoint and send the results and embedding information to Azure AI Studio for measuring the completion quality. To view the completion quality measurements, navigate to the [Azure AI Studio portal](https://ai.azure.com/) and select the project associated with the Azure AI Studio deployment. Select **Evaluation** in the left-hand menu and select the latest evaluation run to view the completion quality measurements.

    ![The completion quality measurements are displayed in the Azure AI Studio portal.](media/example-16-azure-ai-studio.png)
