using SocialChat.Domain.Core.DTO.Users;
using SocialChat.Domain.Core.Models.Users;
using System.Collections.Generic;

namespace SocialChat.Domain.Interfaces.Users
{
    public interface IUserRepository
    {
        int CreateUser(User user);

        UserInfo GetUser(int id);

        int UpdateUser(User user, int id);

        int DeleteUser(int id);

        IEnumerable<UserInfo> GetUsers();

        int CheckIfSameEmailExists(string email);
    }
}
