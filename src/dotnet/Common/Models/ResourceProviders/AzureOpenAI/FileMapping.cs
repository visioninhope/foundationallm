﻿using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.ResourceProviders.AzureOpenAI
{
    /// <summary>
    /// Provides information about a file used by OpenAI assistants.
    /// </summary>
    public class FileMapping
    {
        /// <summary>
        /// The FoundationaLLM.Attachment resource object id.
        /// </summary>
        [JsonPropertyName("foundationallm_attachment_object_id")]
        public required string FoundationaLLMObjectId { get; set; }

        /// <summary>
        /// The original file name of the file.
        /// </summary>
        [JsonPropertyName("original_file_name")]
        public required string OriginalFileName { get; set; }

        /// <summary>
        /// The content type of the file.
        /// </summary>
        [JsonPropertyName("content_type")]
        public required string ContentType { get; set; }

        /// <summary>
        /// The OpenAI file id associated with the FoundationaLLM file (attachment) id.
        /// </summary>
        [JsonPropertyName("openai_file_id")]
        public string? OpenAIFileId { get; set; }

        /// <summary>
        /// The time when the file was uploaded to OpenAI.
        /// </summary>
        [JsonPropertyName("openai_file_uploaded_on")]
        public DateTimeOffset? OpenAIFileUploadedOn { get; set; }

        /// <summary>
        /// Indicates whether the file requires vectorization or not.
        /// </summary>
        [JsonPropertyName("requires_vectorization")]
        public bool RequiresVectorization { get; set; } = false;

        /// <summary>
        /// The OpenAI vector store id holding the vectorized content of the OpenAI file.
        /// </summary>
        [JsonPropertyName("openai_vector_store_id")]
        public string? OpenAIVectorStoreId { get; set; }

        /// <summary>
        /// Indictes whether the file was generated by OpenAI or not.
        /// </summary>
        [JsonPropertyName("generated")]
        public bool Generated { get; set; } = false;

        /// <summary>
        /// The time when the file was generated by OpenAI.
        /// </summary>
        [JsonPropertyName("openai_file_generated_on")]
        public DateTimeOffset? OpenAIFileGeneratedOn { get; set; }
    }
}
