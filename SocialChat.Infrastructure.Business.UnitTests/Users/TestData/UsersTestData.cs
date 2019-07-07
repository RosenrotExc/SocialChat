using Microsoft.AspNetCore.Mvc.ModelBinding;
using SocialChat.Domain.Core.DTO.Users;
using SocialChat.Domain.Core.Models.Users;
using System;
using System.Collections.Generic;

namespace SocialChat.Infrastructure.Business.UnitTests.Users.TestData
{
    static class UsersTestData
    {
        private const char TestChar = '-';
        private const int CharsCount = 10;

        public class CreateUser_TestData
        {
            public List<User> Users = new List<User>
            {
                new User
                {
                    Email = new string(TestChar, CharsCount),
                    FirstName = new string(TestChar, CharsCount),
                    LastName = new string(TestChar, CharsCount),
                    RegistrationDate = DateTime.Now
                }
            };

            public ModelStateDictionary ModelState = new ModelStateDictionary();
        }

        public class DeleteUser_TestData
        {
            public int Id = 1;
        }

        public class GetUser_TestData
        {
            public List<UserInfo> Users = new List<UserInfo>
            {
                new UserInfo
                {
                    Id = 1,
                    Email = new string(TestChar, CharsCount),
                    FirstName = new string(TestChar, CharsCount),
                    LastName = new string(TestChar, CharsCount),
                    RegistrationDate = DateTime.Now
                }
            };
        }

        public class GetUsers_TestData
        {
            public List<UserInfo> Users = new List<UserInfo>
            {
                new UserInfo
                {
                    Id = 1,
                    Email = new string(TestChar, CharsCount),
                    FirstName = new string(TestChar, CharsCount),
                    LastName = new string(TestChar, CharsCount),
                    RegistrationDate = DateTime.Now
                }
            };
        }

        public class UpdateUser_TestData
        {
            public List<User> Users = new List<User>
            {
                new User
                {
                    Email = new string(TestChar, CharsCount),
                    FirstName = new string(TestChar, CharsCount),
                    LastName = new string(TestChar, CharsCount),
                    RegistrationDate = DateTime.Now
                }
            };

            public ModelStateDictionary ModelState = new ModelStateDictionary();

            public int Id = 1;
        }
    }
}
