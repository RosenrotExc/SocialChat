using Microsoft.AspNetCore.Mvc.ModelBinding;
using SocialChat.Domain.Core.Infrastructure;
using SocialChat.Domain.Core.Messages;
using SocialChat.Domain.Core.Messages.Users;
using SocialChat.Domain.Core.Models.Users;
using SocialChat.Domain.Interfaces.Users;
using SocialChat.Services.Interfaces.Users;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Net;

namespace SocialChat.Infrastructure.Business.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserResponse GetUser(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BaseResponse.Failure<UserResponse>(HttpStatusCode.UnprocessableEntity, 
                        Constants.Validation.Users.IncorrectId());
                }

                var user = _userRepository.GetUser(id);

                if (user == null)
                {
                    return BaseResponse.Failure<UserResponse>(HttpStatusCode.NotFound, 
                        Constants.Validation.Users.UserNotFound(id));
                }

                return new UserResponse { User = user };
            }
            catch (SqlException ex)
            {
                return BaseResponse.Failure<UserResponse>(HttpStatusCode.InternalServerError, 
                    Constants.Validation.CommonErrors.SQLError(ex.Message));
            }
            catch (Exception ex)
            {
                return BaseResponse.Failure<UserResponse>(HttpStatusCode.InternalServerError, 
                    Constants.Validation.CommonErrors.ServerError(ex.Message));
            }
        }

        public UserCreateResponse CreateUser(User user, ModelStateDictionary modelState)
        {
            try
            {
                if (user == null)
                {
                    return BaseResponse.Failure<UserCreateResponse>(HttpStatusCode.BadRequest,
                        Constants.Validation.CommonErrors.IncorrectDataProvided());
                }

                if (!modelState.IsValid)
                {
                    var errors = modelState.Values
                        .SelectMany(entry => entry.Errors)
                        .Select(err => err.ErrorMessage)
                        .Aggregate((f, s) => $"{f}; {s}");

                    return BaseResponse.Failure<UserCreateResponse>(HttpStatusCode.BadRequest, errors);
                }

                if (_userRepository.CheckIfSameEmailExists(user.Email) > 0)
                {
                    return BaseResponse.Failure<UserCreateResponse>(HttpStatusCode.Conflict, 
                        Constants.Validation.Users.SameUserExists());
                }

                user.RegistrationDate = DateTime.Now;
                var id = _userRepository.CreateUser(user);

                return new UserCreateResponse { Id = id };
            }
            catch (SqlException ex)
            {
                return BaseResponse.Failure<UserCreateResponse>(HttpStatusCode.InternalServerError, 
                    Constants.Validation.CommonErrors.SQLError(ex.Message));
            }
            catch (Exception ex)
            {
                return BaseResponse.Failure<UserCreateResponse>(HttpStatusCode.InternalServerError, 
                    Constants.Validation.CommonErrors.ServerError(ex.Message));
            }
        }

        public UsersResponse GetUsers()
        {
            try
            {
                var response = _userRepository.GetUsers();

                if (response == null || !response.Any())
                {
                    return BaseResponse.Failure<UsersResponse>(HttpStatusCode.NotFound, 
                        Constants.Validation.Users.UsersNotFound());
                }

                return new UsersResponse { Users = response };
            }
            catch (SqlException ex)
            {
                return BaseResponse.Failure<UsersResponse>(HttpStatusCode.InternalServerError, 
                    Constants.Validation.CommonErrors.SQLError(ex.Message));
            }
            catch (Exception ex)
            {
                return BaseResponse.Failure<UsersResponse>(HttpStatusCode.InternalServerError, 
                    Constants.Validation.CommonErrors.ServerError(ex.Message));
            }
        }

        public BaseResponse DeleteUser(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BaseResponse.Failure(HttpStatusCode.BadRequest, 
                        Constants.Validation.Users.IncorrectId());
                }

                if (_userRepository.DeleteUser(id) > 0)
                {
                    return BaseResponse.Success;
                }

                return BaseResponse.Failure(HttpStatusCode.NotFound, 
                    Constants.Validation.Users.UserNotFound(id));
            }
            catch (SqlException ex)
            {
                return BaseResponse.Failure(HttpStatusCode.InternalServerError, 
                    Constants.Validation.CommonErrors.SQLError(ex.Message));
            }
            catch (Exception ex)
            {
                return BaseResponse.Failure(HttpStatusCode.InternalServerError, 
                    Constants.Validation.CommonErrors.ServerError(ex.Message));
            }
        }

        public BaseResponse UpdateUser(int id, User user, ModelStateDictionary modelState)
        {
            try
            {
                if (user == null)
                {
                    return BaseResponse.Failure<UserCreateResponse>(HttpStatusCode.BadRequest,
                        Constants.Validation.CommonErrors.IncorrectDataProvided());
                }

                if (!modelState.IsValid)
                {
                    var errors = modelState.Values
                        .SelectMany(entry => entry.Errors)
                        .Select(err => err.ErrorMessage)
                        .Aggregate((f, s) => $"{f}; {s}");

                    return BaseResponse.Failure(HttpStatusCode.BadRequest, errors);
                }

                if (_userRepository.CheckIfSameEmailExists(user.Email) > 0)
                {
                    return BaseResponse.Failure(HttpStatusCode.Conflict,
                        Constants.Validation.Users.SameUserExists());
                }

                if (_userRepository.UpdateUser(user, id) > 0)
                {
                    return BaseResponse.Success;
                }

                return BaseResponse.Failure(HttpStatusCode.NotFound,
                    Constants.Validation.Users.UserNotFound(id));
            }
            catch (SqlException ex)
            {
                return BaseResponse.Failure(HttpStatusCode.InternalServerError,
                    Constants.Validation.CommonErrors.SQLError(ex.Message));
            }
            catch (Exception ex)
            {
                return BaseResponse.Failure(HttpStatusCode.InternalServerError,
                    Constants.Validation.CommonErrors.ServerError(ex.Message));
            }
        }
    }
}
