using FoundationaLLM.Common.Models.ResourceProviders.Vectorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Vectorization.Tests.Handlers
{
    public class DeserializationTests
    {
        [Fact]
        public async void TestDeserializeVectorizationRequest()
        {
            string json = @"
            {
                ""id"": ""c15a3ac5-b540-4466-8b9b-fc4cbf94b71c"",
                ""object_id"": ""/instances/a6221c30-0bf2-4003-adb8-d3086bb2ad49/providers/FoundationaLLM.Vectorization/vectorizationRequests/c15a3ac5-b540-4466-8b9b-fc4cbf94b71c"",
                ""expired"": false,
                ""resource_filepath"": ""requests/20240426/20240426-c15a3ac5-b540-4466-8b9b-fc4cbf94b71c.json"",
                ""content_identifier"": {
                    ""data_source_object_id"": ""/instances/a6221c30-0bf2-4003-adb8-d3086bb2ad49/providers/FoundationaLLM.DataSource/dataSources/msdfgm"",
                    ""multipart_id"": [
                        ""dbo"",
                        ""File"",
                        ""FileContents"",
                        ""FileID"",
                        ""350C3136-DEC8-41A3-9375-DF131DFFC6B0"",
                        ""mydoc.docx""
                    ],
                    ""canonical_id"": ""msdfgm/dbo/File/FileContents/FileID/350C3136-DEC8-41A3-9375-DF131DFFC6B0/mydoc.docx""
                },
                ""processing_type"": ""Asynchronous"",
                ""pipeline_object_id"": ""/instances/a6221c30-0bf2-4003-adb8-d3086bb2ad49/providers/FoundationaLLM.Vectorization/vectorizationPipelines/MSDFGM"",
                ""pipeline_execution_id"": ""19fd589e-fcfb-4b1c-997c-8456227a55a7"",
                ""processing_state"": ""New"",
                ""error_messages"": [],
                ""steps"": [
                    {
                        ""id"": ""extract"",
                        ""parameters"": {}
                    },
                    {
                        ""id"": ""partition"",
                        ""parameters"": {
                            ""text_partitioning_profile_name"": ""DefaultTokenTextPartition_Small""
                        }
                    },
                    {
                        ""id"": ""embed"",
                        ""parameters"": {
                            ""text_embedding_profile_name"": ""AzureOpenAI_Embedding_Gateway""
                        }
                    },
                    {
                        ""id"": ""index"",
                        ""parameters"": {
                            ""indexing_profile_name"": ""AzureAISearch_MSDFGM""
                        }
                    }
                ],
                ""completed_steps"": [
                    ""extract"",
                    ""partition""
                ],
                ""remaining_steps"": [
                    ""embed"",
                    ""index""
                ],
                ""current_step"": ""embed"",
                ""error_count"": 0,
                ""running_operations"": {
                    ""embed"": {
                        ""operation_id"": ""edd1197e-43c6-4eaf-aa8b-51eecb67642b"",
                        ""first_response_time"": ""2024-04-26T21:23:15.4092786Z"",
                        ""last_response_time"": ""2024-04-26T21:23:15.4092786Z"",
                        ""complete"": false,
                        ""pollingCount"": 0
                    }
                },
                ""last_successful_step_time"": ""2024-04-26T21:22:14.9690748Z""
            }
            ";

            VectorizationRequest request = JsonSerializer.Deserialize<VectorizationRequest>(json);
            Assert.Equal("c15a3ac5-b540-4466-8b9b-fc4cbf94b71c", request.Id);
        }
    }
}
