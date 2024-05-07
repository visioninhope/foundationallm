using Azure;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using System.Net;

namespace FoundationaLLM.Common.Extensions
{
    /// <summary>
    /// Extends Azure Blob Storage classes with helper methods.
    /// </summary>
    public static class BlobStorageExtensions
    {
        /// <summary>
        /// Attempts to acquire a lease on a blob item using a retry pattern.
        /// </summary>
        /// <param name="blobLeaseClient">The <see cref="BlobLeaseClient"/> client used to acquire the lease.</param>
        /// <param name="duration">The <see cref="TimeSpan"/> specifying the intended duration of the lease.</param>
        /// <param name="maxRetryCount">The maximum number of attempts to acquire the lease.</param>
        /// <param name="retryWaitSeconds">The number of seconds to wait between subsequent attempts to acquire the lease.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that signals the cancellation of the procedure.</param>
        /// <returns>The <see cref="BlobLease"/> acquired or null if the acquire operation was not completed.</returns>
        public static async Task<BlobLease?> AcquireAsyncWithWait(
            this BlobLeaseClient blobLeaseClient,
            TimeSpan duration,
            int maxRetryCount = 5,
            int retryWaitSeconds = 5,
            CancellationToken cancellationToken = default)
        {
            var retryDuration = TimeSpan.FromSeconds(retryWaitSeconds);

            for (var retryCount = 0; retryCount < maxRetryCount; retryCount++)
            {
                try
                {
                    return await blobLeaseClient.AcquireAsync(duration, cancellationToken: cancellationToken);
                }
                catch (RequestFailedException ex)
                {
                    if (ex.Status == (int)HttpStatusCode.Conflict
                        && ex.ErrorCode == "LeaseAlreadyPresent")
                    {
                        // A lease already exists, so we start/continue the retry pattern.

                        if (retryCount == maxRetryCount - 1)
                            // This was the last attempt and the existing lease was still not released.
                            throw;

                        // Wait for the next opportunity to acquire the lease.
                        await Task.Delay(retryDuration);
                    }
                    else
                        throw;
                }
            }

            return null;
        }
    }
}
