using Moq;
using NUnit.Framework;
using SocialChat.Domain.Interfaces.Users;
using SocialChat.Services.Interfaces.Users;
using static SocialChat.Infrastructure.Business.UnitTests.Users.TestData.UsersTestData;
using UserValidationMessages = SocialChat.Domain.Core.Infrastructure.Constants.Validation.Users;
using CommonValidationMessages = SocialChat.Domain.Core.Infrastructure.Constants.Validation.CommonErrors;
using System;
using System.Linq;
using SocialChat.Infrastructure.Business.Users;
using System.Net;
using SocialChat.Domain.Core.Models.Users;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SocialChat.Infrastructure.Business.UnitTests.Users
{
    [TestFixture]
    class UpdateUser
    {
        private IUserService _userService;
        private UpdateUser_TestData TestData;
        private readonly Mock<IUserRepository> _mockUserRepository;

        private User User { get; set; }
        private ModelStateDictionary ModelState { get; set; }
        private int Id { get; set; }

        public UpdateUser()
        {
            _mockUserRepository = new Mock<IUserRepository>();
        }

        [SetUp]
        public void SetUp()
        {
            _userService = new UserService(_mockUserRepository.Object);
            TestData = new UpdateUser_TestData();

            User = TestData.Users.FirstOrDefault();
            ModelState = TestData.ModelState;
            Id = TestData.Id;
        }

        [Test]
        public void UpdateUser_Valid()
        {
            const int updatedCount = 1;
            const int existedCount = 0;
            _mockUserRepository.Setup(repo => repo.CheckIfSameEmailExists(It.IsAny<string>())).Returns(existedCount);
            _mockUserRepository.Setup(repo => repo.UpdateUser(It.IsAny<User>(), It.IsAny<int>())).Returns(updatedCount);

            var actual = _userService.UpdateUser(Id, User, ModelState);

            Assert.IsTrue(actual.Result.Succeeded);
        }

        [Test]
        public void UpdateUser_InvalidModel()
        {
            const string errorKey = "ErrorKey";
            const string errorValue = "ErrorValue";
            ModelState.AddModelError(errorKey, errorValue);

            var actual = _userService.UpdateUser(Id, User, ModelState);

            Assert.IsTrue(actual.Result.Failed);
            Assert.AreEqual(actual.Result.Error.Value, errorValue);
            Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.BadRequest);
        }

        [Test]
        public void UpdateUser_SameUserExists()
        {
            const int existedCount = 1;
            _mockUserRepository.Setup(repo => repo.CheckIfSameEmailExists(It.IsAny<string>())).Returns(existedCount);

            var actual = _userService.UpdateUser(Id, User, ModelState);

            Assert.IsTrue(actual.Result.Failed);
            Assert.AreEqual(actual.Result.Error.Value, UserValidationMessages.SameUserExists());
            Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.Conflict);
        }

        [Test]
        public void UpdateUser_NullRefUser()
        {
            User = null;

            var actual = _userService.UpdateUser(Id, User, ModelState);

            Assert.IsTrue(actual.Result.Failed);
            Assert.AreEqual(actual.Result.Error.Value, CommonValidationMessages.IncorrectDataProvided());
            Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.BadRequest);
        }

        [Test]
        public void UpdateUser_UserNotCreated()
        {
            const int updatedCount = 0;
            const int existedCount = 0;
            _mockUserRepository.Setup(repo => repo.CheckIfSameEmailExists(It.IsAny<string>())).Returns(existedCount);
            _mockUserRepository.Setup(repo => repo.UpdateUser(It.IsAny<User>(), It.IsAny<int>())).Returns(updatedCount);

            var actual = _userService.UpdateUser(Id, User, ModelState);

            Assert.IsTrue(actual.Result.Failed);
            Assert.AreEqual(actual.Result.Error.Value, UserValidationMessages.UserNotFound(Id));
            Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.NotFound);
        }

        [Test]
        public void UpdateUser_ExceptionThrown()
        {
            _mockUserRepository.Setup(repo => repo.CheckIfSameEmailExists(It.IsAny<string>())).Throws<Exception>();

            var actual = _userService.UpdateUser(Id, User, ModelState);

            Assert.IsTrue(actual.Result.Failed);
            Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.InternalServerError);
        }
    }
}
