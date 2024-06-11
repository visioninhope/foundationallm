namespace FoundationaLLM.Core.Examples.Models
{
    /// <summary>
    /// SharePoint Vectorization testing configuration.
    /// </summary>
    public class SharePointVectorizationConfiguration
    {
        /// <summary>
        /// Host name of the SharePoint site without the protocol, ex: fllm.sharepoint.com.
        /// </summary>        
        public string HostName { get; set; } = string.Empty;

        /// <summary>
        /// Relative path of the site/subsite, ex: /sites/FoundationaLLM.
        /// </summary>
        public string SitePath { get; set; } = string.Empty;

        /// <summary>
        /// The folder path, starting with the document library.
        /// </summary>
        public string FolderPath { get; set; } = string.Empty;

        /// <summary>
        /// The file name of the document to vectorize.
        /// </summary>
        public string FileName { get; set; } = string.Empty;
    }
}
