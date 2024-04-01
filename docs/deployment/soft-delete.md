## Which resoureces in FoundationaLLM are soft-deleted?
In FoundationaLLM, the following resources are soft-deleted:
- Azure OpenAI Resources
- Azure Key Vault Resources
- Azure AI Search Resources
- Azure Content Safety Resources

>[!NOTE]
> So be aware that if you don't use the `azd down --purge` command when you want to delete your resources, you will need to do so manually in the portal to purge (delete permenantly) these resources.  Otherwise you will have to make sure to name the resources differently when you redeploy the FoundationaLLM platform.  Another problem you might see is that you might exceed the capacity or tokens allowed for your subscription if you don't purge the resources before creating another one.  So be aware of these issues when you are deploying and deleting resources in your subscription. 