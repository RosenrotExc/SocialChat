using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SocialChat.Domain.Core.Models.Blogs;
using SocialChat.Services.Interfaces.Blogs;

namespace SocialChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : BaseController
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet]
        public IActionResult GetBlog(string id)
        {
            var response = _blogService.GetBlog(id);
            if (response.Result.Succeeded)
            {
                return Ok(response.Blog);
            }

            return StatusCode(response);
        }

        [HttpGet]
        [Route("get-all")]
        public IActionResult GetAllBlogs(IEnumerable<string> ids)
        {
            var response = _blogService.GetBlogs(ids);
            if (response.Result.Succeeded)
            {
                return Ok(response.Blogs);
            }

            return StatusCode(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlog(Blog blog)
        {
            var response = await _blogService.CreateBlogAsync(blog, ModelState);
            if (response.Result.Succeeded)
            {
                return Ok();
            }

            return StatusCode(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBlog(string id)
        {
            var response = await _blogService.DeleteBlogAsync(id);
            if (response.Result.Succeeded)
            {
                return Ok();
            }

            return StatusCode(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBlog(Blog blog)
        {
            var response = await _blogService.UpdateBlogAsync(blog, ModelState);
            if (response.Result.Succeeded)
            {
                return Ok();
            }

            return StatusCode(response);
        }
    }
}