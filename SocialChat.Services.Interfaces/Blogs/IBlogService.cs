using Microsoft.AspNetCore.Mvc.ModelBinding;
using SocialChat.Domain.Core.Messages;
using SocialChat.Domain.Core.Messages.Blogs;
using SocialChat.Domain.Core.Models.Blogs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialChat.Services.Interfaces.Blogs
{
    public interface IBlogService
    {
        Task<BaseResponse> CreateBlogAsync(Blog blog, ModelStateDictionary modelState);

        BlogResponse GetBlog(string id);

        Task<BaseResponse> UpdateBlogAsync(Blog blog, ModelStateDictionary modelState);

        Task<BaseResponse> DeleteBlogAsync(string id);

        BlogsResponse GetBlogs(IEnumerable<string> ids);
    }
}
