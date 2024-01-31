# Triggering Vectorization


## Vectorization request

Sample structure of a vectorization request:

```json
{
    "id": "d4669c9c-e330-450a-a41c-a4d6649abdef",
    "content_identifier": {
        "content_source_profile_name": "SDZWAJournals",
        "multipart_id": [
            "https://fllmaks14sa.blob.core.windows.net",
            "vectorization-input",
            "SDZWA-Journal-January-2024.pdf"
        ],
        "canonical_id": "sdzwa/journals/SDZWA-Journal-January-2024"
    },
    "processing_type": "Asynchronous",
    "steps": [
        {
            "id": "extract",
            "parameters": {}
        },
        {
            "id": "partition",
            "parameters": {
                "text_partition_profile_name": "DefaultTokenTextPartition"
            }
        },
        {
            "id": "embed",
            "parameters": {
                "text_embedding_profile_name": "AzureOpenAI_Embedding"
            }
        },
        {
            "id": "index",
            "parameters": {
                "indexing_profile_name": "AzureAISearch_Test_001"
            }
        }
    ],
    "completed_steps": [],
    "remaining_steps": [
        "extract",
        "partition",
        "embed",
        "index"
    ]
}
```

The `processing_type` property can be one of `Asynchronous` or `Synchronous`. The `Asynchronous` value indicates that the vectorization request is processed asynchronously via the Vectorization workers. The `Synchronous` value indicates that the vectorization request is processed synchronously via the Vectorization API.
