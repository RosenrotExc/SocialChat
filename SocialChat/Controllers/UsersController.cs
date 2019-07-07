using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SocialChat.Domain.Core.Models.Users;
using SocialChat.Services.Interfaces.Users;

namespace SocialChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllOrigin")]
    public class UsersController : BaseController
    {
        public readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetUser(int id)
        {
            var response = _userService.GetUser(id);
            if (response.Result.Succeeded)
            {
                return Ok(response.User);
            }

            return StatusCode(response);
        }

        [HttpGet]
        [Route("get-all")]
        public IActionResult GetAllUsers()
        {
            var response = _userService.GetUsers();
            if (response.Result.Succeeded)
            {
                return Ok(response.Users);
            }

            return StatusCode(response);
        }

        [HttpPost]
        public IActionResult CreateUser(User user)
        {
            var response = _userService.CreateUser(user, ModelState);
            if (response.Result.Succeeded)
            {
                return Ok(response.Id);
            }

            return StatusCode(response);
        }

        [HttpDelete]
        public IActionResult DeleteUser(int id)
        {
            var response = _userService.DeleteUser(id);
            if (response.Result.Succeeded)
            {
                return Ok();
            }

            return StatusCode(response);
        }

        [HttpPut]
        public IActionResult UpdateUser(int id, User user)
        {
            var response = _userService.UpdateUser(id, user, ModelState);
            if (response.Result.Succeeded)
            {
                return Ok();
            }

            return StatusCode(response);
        }
    }
}