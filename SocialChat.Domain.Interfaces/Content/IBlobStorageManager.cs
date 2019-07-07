using Microsoft.AspNetCore.Http;
using SocialChat.Domain.Core.Enums;
using System.Threading.Tasks;

namespace SocialChat.Domain.Interfaces.Content
{
    public interface IBlobStorageManager
    {
        Task<string> UploadFileAsync(IFormFile file, ContentType type, string blobName);

        Task<bool> RemoveContentAsync(string id, ContentType type);
    }
}
