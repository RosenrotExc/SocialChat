using Microsoft.AspNetCore.Mvc;
using SocialChat.Domain.Core.Messages;

namespace SocialChat.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected ObjectResult StatusCode(BaseResponse response) =>
            StatusCode((int)response.Result.Error.Key, response.Result.Error.Value);
    }
}