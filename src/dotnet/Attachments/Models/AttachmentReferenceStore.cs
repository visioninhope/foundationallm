namespace FoundationaLLM.Attachment.Models
{
    /// <summary>
    /// Models the content of the attachment reference store managed by the FoundationaLLM.Attachment resource provider.
    /// </summary>
    public class AttachmentReferenceStore
    {
        /// <summary>
        /// The list of all attachments registered in the system.
        /// </summary>
        public required List<AttachmentReference> AttachmentReferences { get; set; }
        /// <summary>
        /// The name of the default attachment.
        /// </summary>
        public string? DefaultAttachmentName { get; set; }

        /// <summary>
        /// Creates a string-based dictionary of <see cref="AttachmentReference"/> values from the current object.
        /// </summary>
        /// <returns>The string-based dictionary of <see cref="AttachmentReference"/> values from the current object.</returns>
        public Dictionary<string, AttachmentReference> ToDictionary() =>
            AttachmentReferences.ToDictionary<AttachmentReference, string>(ar => ar.Name);

        /// <summary>
        /// Creates a new instance of the <see cref="AttachmentReferenceStore"/> from a dictionary.
        /// </summary>
        /// <param name="dictionary">A string-based dictionary of <see cref="AttachmentReference"/> values.</param>
        /// <returns>The <see cref="AttachmentReferenceStore"/> object created from the dictionary.</returns>
        public static AttachmentReferenceStore FromDictionary(Dictionary<string, AttachmentReference> dictionary) =>
            new()
            {
                AttachmentReferences = [.. dictionary.Values]
            };
    }
}
