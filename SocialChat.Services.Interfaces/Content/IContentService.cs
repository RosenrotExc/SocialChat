using Microsoft.AspNetCore.Http;
using SocialChat.Domain.Core.Enums;
using SocialChat.Domain.Core.Messages;
using SocialChat.Domain.Core.Messages.Content;
using System.Threading.Tasks;

namespace SocialChat.Services.Interfaces.Content
{
    public interface IContentService
    {
        Task<ContentResponse> UploadContentAsync(IFormFile file, ContentType type);

        Task<BaseResponse> RemoveContentAsync(string url, ContentType type);
    }
}
