using Moq;
using NUnit.Framework;
using SocialChat.Domain.Interfaces.Users;
using SocialChat.Services.Interfaces.Users;
using static SocialChat.Infrastructure.Business.UnitTests.Users.TestData.UsersTestData;
using UserValidationMessages = SocialChat.Domain.Core.Infrastructure.Constants.Validation.Users;
using System;
using SocialChat.Infrastructure.Business.Users;
using System.Net;

namespace SocialChat.Infrastructure.Business.UnitTests.Users
{
    [TestFixture]
    class DeleteUser
    {
        private IUserService _userService;
        private DeleteUser_TestData TestData;
        private readonly Mock<IUserRepository> _mockUserRepository;

        private int Id { get; set; }

        public DeleteUser()
        {
            _mockUserRepository = new Mock<IUserRepository>();
        }

        [SetUp]
        public void SetUp()
        {
            _userService = new UserService(_mockUserRepository.Object);
            TestData = new DeleteUser_TestData();
            Id = TestData.Id;
        }

        [Test]
        public void DeleteUser_Valid()
        {
            const int deletedCount = 1;
            _mockUserRepository.Setup(repo => repo.DeleteUser(It.IsAny<int>())).Returns(deletedCount);

            var actual = _userService.DeleteUser(Id);

            Assert.IsTrue(actual.Result.Succeeded);
        }

        [Test]
        [TestCase(0)]
        [TestCase(int.MinValue)]
        public void DeleteUser_BadIdProvided(int id)
        {
            var actual = _userService.DeleteUser(id);

            Assert.IsTrue(actual.Result.Failed);
            Assert.AreEqual(actual.Result.Error.Value, UserValidationMessages.IncorrectId());
            Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.BadRequest);
        }

        [Test]
        public void DeleteUser_NotDeleted()
        {
            const int deletedCount = 0;
            _mockUserRepository.Setup(repo => repo.DeleteUser(It.IsAny<int>())).Returns(deletedCount);

            var actual = _userService.DeleteUser(Id);

            Assert.IsTrue(actual.Result.Failed);
            Assert.AreEqual(actual.Result.Error.Value, UserValidationMessages.UserNotFound(Id));
            Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.NotFound);
        }

        [Test]
        public void DeleteUser_ExceptionThrown()
        {
            _mockUserRepository.Setup(repo => repo.DeleteUser(It.IsAny<int>())).Throws<Exception>();

            var actual = _userService.DeleteUser(Id);

            Assert.IsTrue(actual.Result.Failed);
            Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.InternalServerError);
        }
    }
}
