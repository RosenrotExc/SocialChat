using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using NUnit.Framework;
using System.Linq;
using SocialChat.Domain.Core.Models.Blogs;
using SocialChat.Domain.Interfaces.Blogs;
using SocialChat.Infrastructure.Business.Blogs;
using SocialChat.Services.Interfaces.Blogs;
using System.Threading.Tasks;
using System.Net;
using static SocialChat.Infrastructure.Business.UnitTests.Blogs.TestData.BlogsTestData;
using BlogValidationMessages = SocialChat.Domain.Core.Infrastructure.Constants.Validation.Blogs;
using CommonValidationMessages = SocialChat.Domain.Core.Infrastructure.Constants.Validation.CommonErrors;
using System;

namespace SocialChat.Infrastructure.Business.UnitTests.Blogs
{
    [TestFixture]
    class CreateBlogAsync
    {
        private IBlogService _blogService;
        private CreateBlogAsync_TestData TestData;
        private readonly Mock<IBlogRepository> _mockBlogRepository;

        private Blog Blog { get; set; }
        private ModelStateDictionary ModelState { get; set; }

        public CreateBlogAsync()
        {
            _mockBlogRepository = new Mock<IBlogRepository>();
        }

        [SetUp]
        public void SetUp()
        {
            _blogService = new BlogService(_mockBlogRepository.Object);
            TestData = new CreateBlogAsync_TestData();
            Blog = TestData.Blogs.FirstOrDefault();
            ModelState = TestData.ModelState;
        }

        [Test]
        public async Task CreateBlogAsync_Valid()
        {
            _mockBlogRepository.Setup(repo => repo.CheckForExistedBlog(It.IsAny<string>())).Returns(false);
            _mockBlogRepository.Setup(repo => repo.CreateBlogAsync(It.IsAny<Blog>())).Returns(Task.CompletedTask);

            var actual = await _blogService.CreateBlogAsync(Blog, ModelState);

            Assert.IsTrue(actual.Result.Succeeded);
        }

        [Test]
        public async Task CreateBlogAsync_InvalidModel()
        {
            const string errorKey = "ErrorKey";
            const string errorValue = "ErrorValue";
            ModelState.AddModelError(errorKey, errorValue);

            var actual = await _blogService.CreateBlogAsync(Blog, ModelState);

            Assert.IsTrue(actual.Result.Failed);
            Assert.AreEqual(actual.Result.Error.Value, errorValue);
            Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task CreateBlogAsync_SameBlogExists()
        {
            _mockBlogRepository.Setup(repo => repo.CheckForExistedBlog(It.IsAny<string>())).Returns(true);

            var actual = await _blogService.CreateBlogAsync(Blog, ModelState);

            Assert.IsTrue(actual.Result.Failed);
            Assert.AreEqual(actual.Result.Error.Value, BlogValidationMessages.SameBlogTitleExists());
            Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.Conflict);
        }

        [Test]
        public async Task CreateBlogAsync_NullRefBlog()
        {
            Blog = null;

            var actual = await _blogService.CreateBlogAsync(Blog, ModelState);

            Assert.IsTrue(actual.Result.Failed);
            Assert.AreEqual(actual.Result.Error.Value, CommonValidationMessages.IncorrectDataProvided());
            Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task CreateBlogAsync_ExceptionThrown()
        {
            _mockBlogRepository.Setup(repo => repo.CheckForExistedBlog(It.IsAny<string>())).Throws<Exception>();

            var actual = await _blogService.CreateBlogAsync(Blog, ModelState);

            Assert.IsTrue(actual.Result.Failed);
            Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.InternalServerError);
        }
    }
}
