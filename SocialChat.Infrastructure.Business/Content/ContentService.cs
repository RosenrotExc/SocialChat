using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Web;
using System.IO;
using System.Net;
using Microsoft.WindowsAzure.Storage;
using SocialChat.Services.Interfaces.Content;
using SocialChat.Domain.Interfaces.Content;
using SocialChat.Domain.Core.Messages.Content;
using SocialChat.Domain.Core.Enums;
using SocialChat.Domain.Core.Messages;
using SocialChat.Domain.Core.Infrastructure;

namespace SocialChat.Infrastructure.Business.Content
{
    public class ContentService : IContentService
    {
        private readonly IBlobStorageManager _blobStorageManager;

        public ContentService(IBlobStorageManager blobStorageManager)
        {
            _blobStorageManager = blobStorageManager;
        }

        public async Task<ContentResponse> UploadContentAsync(IFormFile file, ContentType type)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BaseResponse.Failure<ContentResponse>(HttpStatusCode.UnprocessableEntity, 
                        Constants.Validation.Content.InvalidFile(type));
                }

                if (!Constants.Extensions.GetExtensions(type).Any(x => file.FileName.EndsWith(x)))
                {
                    return BaseResponse.Failure<ContentResponse>(HttpStatusCode.UnsupportedMediaType, 
                        Constants.Validation.Content.WrongFileExtension(type));
                }

                var blobName = $@"{CutExtension(file.FileName)}-{Guid.NewGuid()}{ExtractExtension(file.FileName)}";
                var url = await _blobStorageManager.UploadFileAsync(file, type, blobName);

                return new ContentResponse { Url = url };
            }
            catch (Exception ex)
            {
                return BaseResponse.Failure<ContentResponse>(HttpStatusCode.InternalServerError, 
                    Constants.Validation.CommonErrors.ServerError(ex.Message));
            }
        }

        public async Task<BaseResponse> RemoveContentAsync(string url, ContentType type)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(url))
                {
                    return BaseResponse.Failure(HttpStatusCode.BadRequest, 
                        Constants.Validation.Content.WrongUrl(url));
                }

                var contentName = HttpUtility.UrlDecode(new Uri(url).Segments.Last());
                var isDeleted = await _blobStorageManager.RemoveContentAsync(contentName, type);

                if (!isDeleted)
                {
                    return BaseResponse.Failure(HttpStatusCode.NotFound, 
                        Constants.Validation.Content.NotDeleted(url));
                }

                return BaseResponse.Success;
            }
            catch (FormatException)
            {
                return BaseResponse.Failure(HttpStatusCode.UnprocessableEntity, 
                    Constants.Validation.Content.WrongUrl(url));
            }
            catch (StorageException ex)
            {
                return BaseResponse.Failure(HttpStatusCode.InternalServerError, 
                    Constants.Validation.CommonErrors.BlobStorageError(ex.Message));
            }
            catch (Exception ex)
            {
                return BaseResponse.Failure(HttpStatusCode.InternalServerError, 
                    Constants.Validation.CommonErrors.ServerError(ex.Message));
            }
        }

        private string CutExtension(string fileName) 
            => Path.GetFileNameWithoutExtension(fileName);

        private string ExtractExtension(string fileName) 
            => Path.GetExtension(fileName);
    }
}
