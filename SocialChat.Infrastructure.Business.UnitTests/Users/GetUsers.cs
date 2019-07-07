using Moq;
using NUnit.Framework;
using SocialChat.Domain.Interfaces.Users;
using SocialChat.Services.Interfaces.Users;
using static SocialChat.Infrastructure.Business.UnitTests.Users.TestData.UsersTestData;
using UserValidationMessages = SocialChat.Domain.Core.Infrastructure.Constants.Validation.Users;
using System;
using SocialChat.Infrastructure.Business.Users;
using System.Net;
using SocialChat.Domain.Core.DTO.Users;
using System.Collections.Generic;

namespace SocialChat.Infrastructure.Business.UnitTests.Users
{
    [TestFixture]
    class GetUsers
    {
        private IUserService _userService;
        private GetUsers_TestData TestData;
        private readonly Mock<IUserRepository> _mockUserRepository;

        private IEnumerable<UserInfo> Users { get; set; }

        public GetUsers()
        {
            _mockUserRepository = new Mock<IUserRepository>();
        }

        [SetUp]
        public void SetUp()
        {
            _userService = new UserService(_mockUserRepository.Object);
            TestData = new GetUsers_TestData();

            Users = TestData.Users;
        }

        [Test]
        public void GetUsers_Valid()
        {
            _mockUserRepository.Setup(repo => repo.GetUsers()).Returns(Users);

            var actual = _userService.GetUsers();

            Assert.IsTrue(actual.Result.Succeeded);
        }

        [Test]
        public void GetUsers_NullUsersReturned()
        {
            _mockUserRepository.Setup(repo => repo.GetUsers()).Returns<IEnumerable<UserInfo>>(null);

            var actual = _userService.GetUsers();

            Assert.IsTrue(actual.Result.Failed);
            Assert.AreEqual(actual.Result.Error.Value, UserValidationMessages.UsersNotFound());
            Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.NotFound);
        }

        [Test]
        public void GetUsers_EmptyUsersReturned()
        {
            _mockUserRepository.Setup(repo => repo.GetUsers()).Returns(new List<UserInfo>());

            var actual = _userService.GetUsers();

            Assert.IsTrue(actual.Result.Failed);
            Assert.AreEqual(actual.Result.Error.Value, UserValidationMessages.UsersNotFound());
            Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.NotFound);
        }

        [Test]
        public void GetUsers_ExceptionThrown()
        {
            _mockUserRepository.Setup(repo => repo.GetUsers()).Throws<Exception>();

            var actual = _userService.GetUsers();

            Assert.IsTrue(actual.Result.Failed);
            Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.InternalServerError);
        }
    }
}
