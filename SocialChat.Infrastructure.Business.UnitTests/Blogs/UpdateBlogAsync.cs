using Moq;
using NUnit.Framework;
using SocialChat.Domain.Core.Models.Blogs;
using SocialChat.Domain.Interfaces.Blogs;
using SocialChat.Infrastructure.Business.Blogs;
using SocialChat.Services.Interfaces.Blogs;
using System.Net;
using static SocialChat.Infrastructure.Business.UnitTests.Blogs.TestData.BlogsTestData;
using BlogValidationMessages = SocialChat.Domain.Core.Infrastructure.Constants.Validation.Blogs;
using CommonValidationMessages = SocialChat.Domain.Core.Infrastructure.Constants.Validation.CommonErrors;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using System;

namespace SocialChat.Infrastructure.Business.UnitTests.Blogs
{
    [TestFixture]
    class UpdateBlogAsync
    {
        private IBlogService _blogService;
        private UpdateBlogAsync_TestData TestData;
        private readonly Mock<IBlogRepository> _mockBlogRepository;

        private ModelStateDictionary ModelState { get; set; }
        private Blog Blog { get; set; }

        public UpdateBlogAsync()
        {
            _mockBlogRepository = new Mock<IBlogRepository>();
        }

        [SetUp]
        public void SetUp()
        {
            _blogService = new BlogService(_mockBlogRepository.Object);
            TestData = new UpdateBlogAsync_TestData();
            Blog = TestData.Blogs.FirstOrDefault();
            ModelState = TestData.ModelState;
        }

        [Test]
        public async Task UpdateBlogAsync_Valid()
        {
            _mockBlogRepository.Setup(repo => repo.UpdateBlogAsync(It.IsAny<Blog>())).Returns(Task.CompletedTask);
            _mockBlogRepository.Setup(repo => repo.CheckForExistedBlog(It.IsAny<string>())).Returns(false);

            var actual = await _blogService.UpdateBlogAsync(Blog, ModelState);

            Assert.IsTrue(actual.Result.Succeeded);
        }

        [Test]
        public async Task UpdateBlogAsync_InvalidModel()
        {
            const string errorKey = "ErrorKey";
            const string errorValue = "ErrorValue";
            ModelState.AddModelError(errorKey, errorValue);

            var actual = await _blogService.UpdateBlogAsync(Blog, ModelState);

            Assert.IsTrue(actual.Result.Failed);
            Assert.AreEqual(actual.Result.Error.Value, errorValue);
            Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task UpdateBlogAsync_SameBlogExists()
        {
            _mockBlogRepository.Setup(repo => repo.CheckForExistedBlog(It.IsAny<string>())).Returns(true);

            var actual = await _blogService.UpdateBlogAsync(Blog, ModelState);

            Assert.IsTrue(actual.Result.Failed);
            Assert.AreEqual(actual.Result.Error.Value, BlogValidationMessages.SameBlogTitleExists());
            Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.Conflict);
        }

        [Test]
        public async Task UpdateBlogAsync_NullRefBlog()
        {
            Blog = null;

            var actual = await _blogService.UpdateBlogAsync(Blog, ModelState);

            Assert.IsTrue(actual.Result.Failed);
            Assert.AreEqual(actual.Result.Error.Value, CommonValidationMessages.IncorrectDataProvided());
            Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task UpdateBlogAsync_ExceptionThrown()
        {
            _mockBlogRepository.Setup(repo => repo.CheckForExistedBlog(It.IsAny<string>())).Throws<Exception>();

            var actual = await _blogService.UpdateBlogAsync(Blog, ModelState);

            Assert.IsTrue(actual.Result.Failed);
            Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.InternalServerError);
        }
    }
}
