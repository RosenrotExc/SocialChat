using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage.Blob;
using SocialChat.Domain.Core.Enums;
using SocialChat.Domain.Interfaces.Content;
using System.Threading.Tasks;

namespace SocialChat.Infrastructure.Data.Content
{
    public class BlobStorageManager : IBlobStorageManager
    {
        private readonly CloudBlobClient _cloudBlobClient;

        public BlobStorageManager(CloudBlobClient cloudBlobClient)
        {
            _cloudBlobClient = cloudBlobClient;
        }

        public async Task<string> UploadFileAsync(IFormFile file, ContentType type, string blobName)
        {
            var _blobContainer = _cloudBlobClient.GetContainerReference(type.ToString().ToLower());
            await _blobContainer.CreateIfNotExistsAsync();
            var blockBlob = _blobContainer.GetBlockBlobReference(blobName);
            await blockBlob.UploadFromStreamAsync(file.OpenReadStream());

            return blockBlob.Uri.AbsoluteUri;
        }

        public async Task<bool> RemoveContentAsync(string id, ContentType type)
        {
            var _blobContainer = _cloudBlobClient.GetContainerReference(type.ToString().ToLower());
            await _blobContainer.CreateIfNotExistsAsync();

            return await _blobContainer.GetBlockBlobReference(id).DeleteIfExistsAsync();
        }
    }
}
