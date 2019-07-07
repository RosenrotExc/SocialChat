using System.Data;
using System.Collections.Generic;
using SocialChat.Domain.Interfaces.Users;
using SocialChat.Domain.Core.Models.Users;
using SocialChat.Domain.Core.DTO.Users;

namespace SocialChat.Infrastructure.Data.Users
{
    public class UserRepository : BaseDbAccess, IUserRepository
    {
        public UserRepository(IDbConnection connect) : base(connect) { }

        public int CheckIfSameEmailExists(string email)
        {
            return Connection.QueryFirstOrDefaultSf<int>(@"SELECT [dbo].[USERS_CheckExistedOne]('@email')", new { email });
        }

        public int CreateUser(User user)
        {
            return Connection.QueryFirstOrDefaultSp<int>(@"[dbo].[USERS_CreateUser]", user);
        }

        public int DeleteUser(int id)
        {
            return Connection.QueryFirstOrDefaultSp<int>(@"[dbo].[USERS_DeleteUser]", new { id });
        }

        public UserInfo GetUser(int id)
        {
            return Connection.QueryFirstOrDefaultSp<UserInfo>(@"[dbo].[USERS_GetUser]", new { id });
        }

        public IEnumerable<UserInfo> GetUsers()
        {
            return Connection.QuerySp<UserInfo>(@"[dbo].[USERS_GetUsers]");
        }

        public int UpdateUser(User user, int Id)
        {
            return Connection.QueryFirstOrDefaultSp<int>(@"[dbo].[USERS_UpdateUser]", new
            {
                user.Email,
                user.FirstName,
                user.LastName,
                Id
            });
        }
    }
}
