using Moq;
using NUnit.Framework;
using SocialChat.Domain.Core.Models.Users;
using SocialChat.Domain.Interfaces.Users;
using SocialChat.Services.Interfaces.Users;
using static SocialChat.Infrastructure.Business.UnitTests.Users.TestData.UsersTestData;
using CommonValidationMessages = SocialChat.Domain.Core.Infrastructure.Constants.Validation.CommonErrors;
using UserValidationMessages = SocialChat.Domain.Core.Infrastructure.Constants.Validation.Users;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SocialChat.Infrastructure.Business.Users;
using System.Linq;
using System.Net;

namespace SocialChat.Infrastructure.Business.UnitTests.Users
{
    [TestFixture]
    class CreateUser
    {
        private IUserService _userService;
        private CreateUser_TestData TestData;
        private readonly Mock<IUserRepository> _mockUserRepository;

        private User User { get; set; }
        private ModelStateDictionary ModelState { get; set; }

        public CreateUser()
        {
            _mockUserRepository = new Mock<IUserRepository>();
        }

        [SetUp]
        public void SetUp()
        {
            _userService = new UserService(_mockUserRepository.Object);
            TestData = new CreateUser_TestData();
            User = TestData.Users.FirstOrDefault();
            ModelState = TestData.ModelState;
        }

        [Test]
        public void CreateUser_Valid()
        {
            _mockUserRepository.Setup(repo => repo.CheckIfSameEmailExists(It.IsAny<string>())).Returns(0);
            _mockUserRepository.Setup(repo => repo.CreateUser(It.IsAny<User>())).Returns(It.IsAny<int>());

            var actual = _userService.CreateUser(User, ModelState);

            Assert.IsTrue(actual.Result.Succeeded);
        }

        [Test]
        public void CreateUser_InvalidModel()
        {
            const string errorKey = "ErrorKey";
            const string errorValue = "ErrorValue";
            ModelState.AddModelError(errorKey, errorValue);

            var actual = _userService.CreateUser(User, ModelState);

            Assert.IsTrue(actual.Result.Failed);
            Assert.AreEqual(actual.Result.Error.Value, errorValue);
            Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.BadRequest);
        }

        [Test]
        public void CreateUser_SameUserExists()
        {
            const int existedUsers = 1;
            _mockUserRepository.Setup(repo => repo.CheckIfSameEmailExists(It.IsAny<string>())).Returns(existedUsers);

            var actual = _userService.CreateUser(User, ModelState);

            Assert.IsTrue(actual.Result.Failed);
            Assert.AreEqual(actual.Result.Error.Value, UserValidationMessages.SameUserExists());
            Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.Conflict);
        }

        [Test]
        public void CreateUser_NullRefUser()
        {
            User = null;

            var actual = _userService.CreateUser(User, ModelState);

            Assert.IsTrue(actual.Result.Failed);
            Assert.AreEqual(actual.Result.Error.Value, CommonValidationMessages.IncorrectDataProvided());
            Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.BadRequest);
        }

        [Test]
        public void CreateUser_ExceptionThrown()
        {
            _mockUserRepository.Setup(repo => repo.CheckIfSameEmailExists(It.IsAny<string>())).Throws<Exception>();

            var actual = _userService.CreateUser(User, ModelState);

            Assert.IsTrue(actual.Result.Failed);
            Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.InternalServerError);
        }
    }
}
