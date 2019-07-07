using SocialChat.Domain.Core.DTO.Users;
using System.Collections.Generic;

namespace SocialChat.Domain.Core.Messages.Users
{
    public class UsersResponse : BaseResponse
    {
        public IEnumerable<UserInfo> Users { get; set; }
    }
}
