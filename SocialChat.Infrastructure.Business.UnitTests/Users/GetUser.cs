using Moq;
using NUnit.Framework;
using SocialChat.Domain.Interfaces.Users;
using SocialChat.Services.Interfaces.Users;
using static SocialChat.Infrastructure.Business.UnitTests.Users.TestData.UsersTestData;
using UserValidationMessages = SocialChat.Domain.Core.Infrastructure.Constants.Validation.Users;
using System;
using System.Linq;
using SocialChat.Infrastructure.Business.Users;
using System.Net;
using SocialChat.Domain.Core.DTO.Users;

namespace SocialChat.Infrastructure.Business.UnitTests.Users
{
    [TestFixture]
    class GetUser
    {
        private IUserService _userService;
        private GetUser_TestData TestData;
        private readonly Mock<IUserRepository> _mockUserRepository;

        private UserInfo User { get; set; }

        public GetUser()
        {
            _mockUserRepository = new Mock<IUserRepository>();
        }

        [SetUp]
        public void SetUp()
        {
            _userService = new UserService(_mockUserRepository.Object);
            TestData = new GetUser_TestData();

            User = TestData.Users.FirstOrDefault();
        }

        [Test]
        public void GetUser_Valid()
        {
            _mockUserRepository.Setup(repo => repo.GetUser(It.IsAny<int>())).Returns(User);

            var actual = _userService.GetUser(User.Id);

            Assert.IsTrue(actual.Result.Succeeded);
        }

        [Test]
        [TestCase(0)]
        [TestCase(int.MinValue)]
        public void GetUser_BadIdProvided(int id)
        {
            var actual = _userService.GetUser(id);

            Assert.IsTrue(actual.Result.Failed);
            Assert.AreEqual(actual.Result.Error.Value, UserValidationMessages.IncorrectId());
            Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.UnprocessableEntity);
        }

        [Test]
        public void GetUser_UserNullReturned()
        {
            _mockUserRepository.Setup(repo => repo.GetUser(It.IsAny<int>())).Returns<UserInfo>(null);

            var actual = _userService.GetUser(User.Id);

            Assert.IsTrue(actual.Result.Failed);
            Assert.AreEqual(actual.Result.Error.Value, UserValidationMessages.UserNotFound(User.Id));
            Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.NotFound);
        }

        [Test]
        public void GetUser_ExceptionThrown()
        {
            _mockUserRepository.Setup(repo => repo.GetUser(It.IsAny<int>())).Throws<Exception>();

            var actual = _userService.GetUser(User.Id);

            Assert.IsTrue(actual.Result.Failed);
            Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.InternalServerError);
        }
    }
}
