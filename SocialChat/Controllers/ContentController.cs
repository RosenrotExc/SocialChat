using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialChat.Domain.Core.Enums;
using SocialChat.Services.Interfaces.Content;
using SocialChat.Domain.Core.Infrastructure;

namespace SocialChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController : BaseController
    {
        private readonly IContentService _contentService;

        public ContentController(IContentService contentService)
        {
            _contentService = contentService;
        }

        [HttpPost]
        [RequestSizeLimit(Constants.Validation.Content.MaxAllowedFileSize)]
        public async Task<IActionResult> UploadContentAsync([FromForm]IFormFile file, ContentType type)
        {
            var response = await _contentService.UploadContentAsync(file, type);
            if (response.Result.Succeeded)
            {
                return Ok(response.Url);
            }

            return StatusCode(response);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveContentAsync(string url, ContentType type)
        {
            var response = await _contentService.RemoveContentAsync(url, type);
            await _contentService.RemoveContentAsync(null, type);
            if (response.Result.Succeeded)
            {
                return Ok();
            }

            return StatusCode(response);
        }
    }
}