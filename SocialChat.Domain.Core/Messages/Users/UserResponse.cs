using SocialChat.Domain.Core.DTO.Users;

namespace SocialChat.Domain.Core.Messages.Users
{
    public class UserResponse : BaseResponse
    {
        public UserInfo User { get; set; }
    }
}
