using Moq;
using NUnit.Framework;
using SocialChat.Domain.Core.Models.Blogs;
using SocialChat.Domain.Interfaces.Blogs;
using SocialChat.Infrastructure.Business.Blogs;
using SocialChat.Services.Interfaces.Blogs;
using System.Net;
using static SocialChat.Infrastructure.Business.UnitTests.Blogs.TestData.BlogsTestData;
using BlogValidationMessages = SocialChat.Domain.Core.Infrastructure.Constants.Validation.Blogs;
using System;
using System.Collections.Generic;

namespace SocialChat.Infrastructure.Business.UnitTests.Blogs
{
    [TestFixture]
    class GetBlogs
    {
        private IBlogService _blogService;
        private GetBlogs_TestData TestData;
        private readonly Mock<IBlogRepository> _mockBlogRepository;

        private IEnumerable<string> Ids { get; set; }
        private IEnumerable<Blog> Blogs { get; set; }

        public GetBlogs()
        {
            _mockBlogRepository = new Mock<IBlogRepository>();
        }

        [SetUp]
        public void SetUp()
        {
            _blogService = new BlogService(_mockBlogRepository.Object);
            TestData = new GetBlogs_TestData();
            Ids = TestData.Ids;
            Blogs = TestData.Blogs;
        }

        [Test]
        public void GetBlogs_Valid()
        {
            _mockBlogRepository.Setup(repo => repo.GetBlogs(It.IsAny<IEnumerable<string>>())).Returns(Blogs);

            var actual = _blogService.GetBlogs(Ids);

            Assert.IsTrue(actual.Result.Succeeded);
            Assert.NotNull(actual.Blogs);
        }

        [Test]
        [TestCase(null)]
        public void GetBlogs_NullIdsProvided(IEnumerable<string> ids)
        {
            var actual = _blogService.GetBlogs(ids);

            Assert.IsTrue(actual.Result.Failed);
            Assert.AreEqual(actual.Result.Error.Value, BlogValidationMessages.NoIdsRecieved());
            Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.BadRequest);
        }

        [Test]
        public void GetBlogs_EmptyIdsProvided()
        {
            var actual = _blogService.GetBlogs(new List<string>());

            Assert.IsTrue(actual.Result.Failed);
            Assert.AreEqual(actual.Result.Error.Value, BlogValidationMessages.NoIdsRecieved());
            Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.BadRequest);
        }

        [Test]
        [TestCase(null)]
        public void GetBlogs_NullBlogReturned(IEnumerable<Blog> blogs)
        {
            _mockBlogRepository.Setup(repo => repo.GetBlogs(It.IsAny<IEnumerable<string>>())).Returns(blogs);

            var actual = _blogService.GetBlogs(Ids);

            Assert.IsTrue(actual.Result.Failed);
            Assert.AreEqual(actual.Result.Error.Value, BlogValidationMessages.BlogsNotFound());
            Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.NotFound);
        }

        [Test]
        public void GetBlogs_EmptyBlogsReturned()
        {
            _mockBlogRepository.Setup(repo => repo.GetBlogs(It.IsAny<IEnumerable<string>>())).Returns(new List<Blog>());

            var actual = _blogService.GetBlogs(Ids);

            Assert.IsTrue(actual.Result.Failed);
            Assert.AreEqual(actual.Result.Error.Value, BlogValidationMessages.BlogsNotFound());
            Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.NotFound);
        }

        [Test]
        public void GetBlogs_ExceptionThrown()
        {
            _mockBlogRepository.Setup(repo => repo.GetBlogs(It.IsAny<IEnumerable<string>>())).Throws<Exception>();

            var actual = _blogService.GetBlogs(Ids);

            Assert.IsTrue(actual.Result.Failed);
            Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.InternalServerError);
        }
    }
}
