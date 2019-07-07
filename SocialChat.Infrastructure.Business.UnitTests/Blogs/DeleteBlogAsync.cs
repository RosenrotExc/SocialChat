using Moq;
using NUnit.Framework;
using SocialChat.Domain.Interfaces.Blogs;
using SocialChat.Infrastructure.Business.Blogs;
using SocialChat.Services.Interfaces.Blogs;
using System.Threading.Tasks;
using System.Net;
using static SocialChat.Infrastructure.Business.UnitTests.Blogs.TestData.BlogsTestData;
using BlogValidationMessages = SocialChat.Domain.Core.Infrastructure.Constants.Validation.Blogs;
using System;

namespace SocialChat.Infrastructure.Business.UnitTests.Blogs
{
    [TestFixture]
    class DeleteBlogAsync
    {
        private IBlogService _blogService;
        private DeleteBlogAsync_TestData TestData;
        private readonly Mock<IBlogRepository> _mockBlogRepository;

        private string Id { get; set; }

        public DeleteBlogAsync()
        {
            _mockBlogRepository = new Mock<IBlogRepository>();
        }

        [SetUp]
        public void SetUp()
        {
            _blogService = new BlogService(_mockBlogRepository.Object);
            TestData = new DeleteBlogAsync_TestData();
            Id = TestData.Id;
        }

        [Test]
        public async Task DeleteBlogAsync_Valid()
        {
            const long deletedCount = 1;
            _mockBlogRepository.Setup(repo => repo.DeleteBlogAsync(It.IsAny<string>())).Returns(Task.FromResult(deletedCount));

            var actual = await _blogService.DeleteBlogAsync(Id);

            Assert.IsTrue(actual.Result.Succeeded);
        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public async Task DeleteBlogAsync_BadIdProvided(string id)
        {
            var actual = await _blogService.DeleteBlogAsync(id);

            Assert.IsTrue(actual.Result.Failed);
            Assert.AreEqual(actual.Result.Error.Value, BlogValidationMessages.IncorrectId());
            Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task DeleteBlogAsync_NotDeleted()
        {
            const long deletedCount = 0;
            _mockBlogRepository.Setup(repo => repo.DeleteBlogAsync(It.IsAny<string>())).Returns(Task.FromResult(deletedCount));

            var actual = await _blogService.DeleteBlogAsync(Id);

            Assert.IsTrue(actual.Result.Failed);
            Assert.AreEqual(actual.Result.Error.Value, BlogValidationMessages.BlogNotFound(Id));
            Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.NotFound);
        }

        [Test]
        public async Task DeleteBlogAsync_ExceptionThrown()
        {
            _mockBlogRepository.Setup(repo => repo.DeleteBlogAsync(It.IsAny<string>())).Throws<Exception>();

            var actual = await _blogService.DeleteBlogAsync(Id);

            Assert.IsTrue(actual.Result.Failed);
            Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.InternalServerError);
        }
    }
}
