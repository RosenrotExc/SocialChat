using Moq;
using NUnit.Framework;
using System.Linq;
using SocialChat.Domain.Core.Models.Blogs;
using SocialChat.Domain.Interfaces.Blogs;
using SocialChat.Infrastructure.Business.Blogs;
using SocialChat.Services.Interfaces.Blogs;
using System.Net;
using static SocialChat.Infrastructure.Business.UnitTests.Blogs.TestData.BlogsTestData;
using BlogValidationMessages = SocialChat.Domain.Core.Infrastructure.Constants.Validation.Blogs;
using System;

namespace SocialChat.Infrastructure.Business.UnitTests.Blogs
{
    [TestFixture]
    class GetBlog
    {
        private IBlogService _blogService;
        private GetBlog_TestData TestData;
        private readonly Mock<IBlogRepository> _mockBlogRepository;

        private string Id { get; set; }
        private Blog Blog { get; set; }

        public GetBlog()
        {
            _mockBlogRepository = new Mock<IBlogRepository>();
        }

        [SetUp]
        public void SetUp()
        {
            _blogService = new BlogService(_mockBlogRepository.Object);
            TestData = new GetBlog_TestData();
            Id = TestData.Id;
            Blog = TestData.Blogs.FirstOrDefault();
        }

        [Test]
        public void GetBlog_Valid()
        {
            _mockBlogRepository.Setup(repo => repo.GetBlog(It.IsAny<string>())).Returns(Blog);

            var actual = _blogService.GetBlog(Id);

            Assert.IsTrue(actual.Result.Succeeded);
            Assert.NotNull(actual.Blog);
        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void GetBlog_BadIdProvided(string id)
        {
            var actual = _blogService.GetBlog(id);

            Assert.IsTrue(actual.Result.Failed);
            Assert.AreEqual(actual.Result.Error.Value, BlogValidationMessages.IncorrectId());
            Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.BadRequest);
        }

        [Test]
        public void GetBlog_NotFound()
        {
            _mockBlogRepository.Setup(repo => repo.GetBlog(It.IsAny<string>())).Returns<Blog>(null);

            var actual = _blogService.GetBlog(Id);

            Assert.IsTrue(actual.Result.Failed);
            Assert.AreEqual(actual.Result.Error.Value, BlogValidationMessages.BlogNotFound(Id));
            Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.NotFound);
        }

        [Test]
        public void GetBlog_ExceptionThrown()
        {
            _mockBlogRepository.Setup(repo => repo.GetBlog(It.IsAny<string>())).Throws<Exception>();

            var actual = _blogService.GetBlog(Id);

            Assert.IsTrue(actual.Result.Failed);
            Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.InternalServerError);
        }
    }
}
