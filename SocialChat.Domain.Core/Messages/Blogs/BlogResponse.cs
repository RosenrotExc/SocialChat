using SocialChat.Domain.Core.Models.Blogs;

namespace SocialChat.Domain.Core.Messages.Blogs
{
    public class BlogResponse : BaseResponse
    {
        public Blog Blog { get; set; }
    }
}
