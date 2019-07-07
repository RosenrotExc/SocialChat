using SocialChat.Domain.Core.Models.Blogs;
using System.Collections.Generic;

namespace SocialChat.Domain.Core.Messages.Blogs
{
    public class BlogsResponse : BaseResponse
    {
        public IEnumerable<Blog> Blogs { get; set; }
    }
}
