namespace FarmersAppWithSearch.Models
{
    public class AzureBlobStorageSettings
    // connection string and container name for Azure storage account
    {
        public string ConnectionString { get; set; } = string.Empty;


        // Name of the blob container
        public string ContainerName { get; set; } = string.Empty;
    }
}
