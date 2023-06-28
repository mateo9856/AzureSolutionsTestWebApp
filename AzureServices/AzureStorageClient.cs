using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureServices
{
    public class AzureStorageClient : IDisposable
    {
        public string AccountName { get; set; }
        public string LastContainerName { get; set; } = null;
        public string LastBlobName { get; set; }
        public BlobServiceClient BlobClient { get; set; }
        public BlobClientOptions BlobClientOptions { get; set; }
        public AzureStorageClient(string accountName = "default")
        {
            BlobClient = new BlobServiceClient(new Uri($"https://{accountName}.blob.core.windows.net"), new DefaultAzureCredential());
        }

        public async Task<List<string>> GetBlobsListFromContainerAsync(string containerName)
        {
            var blobList = new List<string>();
            var container = GetContainerClient(containerName);
            await foreach(BlobItem item in container.GetBlobsAsync())
            {
                blobList.Add(item.Name);
            }
            return blobList;
        }

        public async Task CreateContainerAsync(string container) => await BlobClient.CreateBlobContainerAsync(container);

        public BlobContainerClient GetContainerClient(string containerName, BlobClientOptions options = null) {
            LastContainerName = containerName;
            return new BlobContainerClient(new Uri($"https://{AccountName}.blob.core.windows.net/{containerName}"), new DefaultAzureCredential(), options);
        }
        public BlobClient GetBlobClient(string BlobName) {
            LastBlobName = BlobName;
            return GetContainerClient(LastContainerName).GetBlobClient(BlobName);
        }

        public async Task<bool> DeleteContainerAsync(string container)
        {
            var response = await GetContainerClient(container).DeleteAsync();
            if (response.IsError)
                return false;
            else
            {
                if (container == LastContainerName)
                    LastContainerName = String.Empty;
                return true;
            }
        }

        public async void SetContainerMetaData(IDictionary<string, string> metadata, string containerName) => await GetContainerClient(containerName).SetMetadataAsync(metadata);

        public async Task UploadToContainerAsync(string folderDirectory, string file)
        {
            var pathDirectory = Path.Combine(folderDirectory, file);
            var blobClient = GetBlobClient(LastBlobName);
            using (FileStream fs = File.OpenRead(pathDirectory))
            {
                await blobClient.UploadAsync(fs);
                fs.Close();
            }
        }

        public void Dispose()
        {
            BlobClient = null;
            BlobClientOptions = null;
        }
    }
}
