using FoundationaLLM.Common.Models.Vectorization;

namespace FoundationaLLM.Vectorization.Interfaces
{
    /// <summary>
    /// Provides access to items in a data source.
    /// </summary>
    public interface IContentSourceService
    {
        /// <summary>
        /// Reads the content of a data source item.
        /// </summary>
        /// <param name="contentId">The <see cref="ContentIdentifier"/> providing the unique identifier of the item being read.</param>
        /// <param name="cancellationToken">The cancellation token that signals that operations should be cancelled.</param>
        /// <returns>The string content of the item.</returns>
        Task<string> ExtractTextAsync(ContentIdentifier contentId, CancellationToken cancellationToken);
    }
}
