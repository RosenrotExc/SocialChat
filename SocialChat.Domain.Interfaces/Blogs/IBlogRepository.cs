using SocialChat.Domain.Core.Models.Blogs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialChat.Domain.Interfaces.Blogs
{
    public interface IBlogRepository
    {
        Task CreateBlogAsync(Blog blog);

        Blog GetBlog(string id);

        Task UpdateBlogAsync(Blog blog);

        Task<long> DeleteBlogAsync(string id);

        IEnumerable<Blog> GetBlogs(IEnumerable<string> ids);

        bool CheckForExistedBlog(string title);
    }
}
