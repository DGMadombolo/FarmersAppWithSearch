using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BlobStorageWebAppMVC.Models;
using Microsoft.Extensions.Options;

namespace BlobStorageMVC.Services
{
    public class BlobService
    {
        private readonly AzureBlobStorageSettings _setting;

        public BlobService(IOptions<AzureBlobStorageSettings> setting)
        {
            _setting = setting.Value;
        }

        public async Task<string> UploadImageAync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is null or empty.");
            var blobServiceClient = new BlobServiceClient(_setting.ConnectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(_setting.ContainerName);

            //generate unique name for the file

            string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var blobClient = blobContainerClient.GetBlobClient(filename);

            //upload file with content type
            using (var stream = file.OpenReadStream())
            {
                var header = new BlobHttpHeaders
                {
                    ContentType = file.ContentType //this will ensure that the file is served with the correct content type when accessed
                };

                await blobClient.UploadAsync(stream, new BlobUploadOptions
                {
                    HttpHeaders = new BlobHttpHeaders { ContentType = file.ContentType }
                });
            }

            return blobClient.Uri.ToString();

        }
        public async Task<List<VenueViewModel>> GetAllImagesAsync()
        {
            var venue = new List<VenueViewModel>();


            var blobServiceClient = new BlobServiceClient(_setting.ConnectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(_setting.ContainerName);

            await foreach (var blobItem in blobContainerClient.GetBlobsAsync())
            {
                var blobClient = blobContainerClient.GetBlobClient(blobItem.Name);
                venue.Add(new VenueViewModel
                {
                    Name = Path.GetFileNameWithoutExtension(blobItem.Name),
                    ImageUrl = blobClient.Uri.ToString()
                });
            }

            return venue;


        }
    }

}