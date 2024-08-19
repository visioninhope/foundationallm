using FoundationaLLM.Common.Models.Authentication;
using FoundationaLLM.Common.Models.ResourceProviders.Attachment;
using FoundationaLLM.Common.Models.ResourceProviders.AzureOpenAI;

namespace Core.Examples.LoadTests.Data
{
    public static class LoadTestData
    {
        private const int SimulatedUsersCount = 20;
        private const int SimulatedFileCount = 10;

        public static List<UnifiedUserIdentity> GetUserIdentities(int hostId) =>
            Enumerable.Range(1, SimulatedUsersCount)
                .Select(i => new UnifiedUserIdentity
                {
                    GroupIds = ["00000000-0000-0000-0000-000000000001"],
                    UserId = $"00000000-0000-0000-{hostId:D4}-{i:D12}",
                    Username = $"load_test_user_{hostId:D3}_{i:D3}@solliance.net",
                    Name = $"Load Test User {hostId:D3}-{i:D3}",
                    UPN = $"load_test_user_{hostId:D3}_{i:D3}@solliance.net"
                })
                .ToList();

        public static List<AttachmentFile> GetAttachmentFiles(string userId) =>
            Enumerable.Range(1, SimulatedFileCount)
                .Select(i => new AttachmentFile
                {
                    Name = $"a-{userId}-{i:D18}",
                    Content = new byte[] { 0x20 },
                    DisplayName = "test_original_file_name",
                    ContentType = "application/octet-stream",
                    OriginalFileName = "test_original_file_name.jpg"
                })
                .ToList();

        public static FileUserContext GetFileUserContext(string userName, string instanceId, string objectId, AttachmentFile attachmentFile) =>
           new FileUserContext
           {
               Name = $"{userName}-file-{instanceId.ToLower()}",
               UserPrincipalName = userName!,
               Endpoint = "https://fllm-01.openai.azure.com/",
               AssistantUserContextName = $"{userName}-assistant-{instanceId.ToLower()}",
               Files = new()
                {
                    {
                        objectId,
                        new FileMapping
                        {
                            FoundationaLLMObjectId = objectId!,
                            OriginalFileName = attachmentFile.OriginalFileName,
                            ContentType = attachmentFile.ContentType!
                        }
                    }
                }
           };
    }
}
