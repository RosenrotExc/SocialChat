using Microsoft.AspNetCore.Mvc.ModelBinding;
using SocialChat.Domain.Core.Messages;
using SocialChat.Domain.Core.Messages.Users;
using SocialChat.Domain.Core.Models.Users;

namespace SocialChat.Services.Interfaces.Users
{
    public interface IUserService
    {
        UserCreateResponse CreateUser(User user, ModelStateDictionary modelState);

        UserResponse GetUser(int id);

        BaseResponse UpdateUser(int id, User user, ModelStateDictionary modelState);

        BaseResponse DeleteUser(int id);

        UsersResponse GetUsers();
    }
}
