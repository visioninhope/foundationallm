namespace FoundationaLLM.Upgrade.Models._050
{
    /// <summary>
    /// Types of content sources from which documents can be retrieved.
    /// </summary>
    public enum ContentSourceType050
    {
        /// <summary>
        /// Azure data lake storage account.
        /// </summary>
        AzureDataLake,

        /// <summary>
        /// SharePoint Online document library.
        /// </summary>
        SharePointOnline,

        /// <summary>
        /// Azure SQL Database.
        /// </summary>
        AzureSQLDatabase,

        /// <summary>
        /// Web page.
        /// </summary>
        Web
    }
}
