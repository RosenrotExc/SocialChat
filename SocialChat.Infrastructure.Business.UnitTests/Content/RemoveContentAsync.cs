using Moq;
using NUnit.Framework;
using SocialChat.Domain.Core.Enums;
using SocialChat.Domain.Interfaces.Content;
using SocialChat.Infrastructure.Business.Content;
using SocialChat.Services.Interfaces.Content;
using System;
using System.Threading.Tasks;
using SocialChat.Domain.Core.Infrastructure;
using static SocialChat.Infrastructure.Business.UnitTests.Content.TestData.ContentTestData;
using System.Net;

namespace SocialChat.Infrastructure.Business.UnitTests.Content
{
    class RemoveContentAsync
    {
        private IContentService _contentService;
        private RemoveContentAsync_TestData TestData;
        private readonly Mock<IBlobStorageManager> _mockBlobStorageManager;

        private string Url { get; set; }

        public RemoveContentAsync()
        {
            _mockBlobStorageManager = new Mock<IBlobStorageManager>();
        }

        [SetUp]
        public void SetUp()
        {
            _contentService = new ContentService(_mockBlobStorageManager.Object);
            TestData = new RemoveContentAsync_TestData();
            Url = TestData.TestUrl;
        }

        [Test]
        [TestCaseSource(typeof(Constants.Extensions), "ImageExtensions")]
        public async Task RemoveContentAsync_ImageValid(string extension)
        {
            const ContentType type = ContentType.Image;
            const bool isSuccessed = true;
            var url = $"{Url}{extension}";
            _mockBlobStorageManager
                .Setup(repo => repo.RemoveContentAsync(It.IsAny<string>(), type))
                .Returns(Task.FromResult(isSuccessed));

            var actual = await _contentService.RemoveContentAsync(url, type);

            Assert.IsTrue(actual.Result.Succeeded);
        }

        [Test]
        [TestCaseSource(typeof(Constants.Extensions), "VideoExtensions")]
        public async Task RemoveContentAsync_VideoValid(string extension)
        {
            const ContentType type = ContentType.Video;
            const bool isSuccessed = true;
            var url = $"{Url}{extension}";
            _mockBlobStorageManager
                .Setup(repo => repo.RemoveContentAsync(It.IsAny<string>(), type))
                .Returns(Task.FromResult(isSuccessed));

            var actual = await _contentService.RemoveContentAsync(url, type);

            Assert.IsTrue(actual.Result.Succeeded);
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public async Task RemoveContentAsync_InvalidUrl(string url)
        {
            var actual = await _contentService.RemoveContentAsync(url, It.IsAny<ContentType>());

            Assert.IsTrue(actual.Result.Failed);
            Assert.AreEqual(actual.Result.Error.Value, Constants.Validation.Content.WrongUrl(url));
            Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.BadRequest);
        }

        [Test]
        [TestCaseSource(typeof(Constants.Extensions), "ImageExtensions")]
        public async Task RemoveContentAsync_ImageNotDeleted(string extension)
        {
            const ContentType type = ContentType.Image;
            const bool isSuccessed = false;
            var url = $"{Url}{extension}";
            _mockBlobStorageManager
                .Setup(repo => repo.RemoveContentAsync(It.IsAny<string>(), type))
                .Returns(Task.FromResult(isSuccessed));

            var actual = await _contentService.RemoveContentAsync(url, It.IsAny<ContentType>());

            Assert.IsTrue(actual.Result.Failed);
            Assert.AreEqual(actual.Result.Error.Value, Constants.Validation.Content.NotDeleted(url));
            Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.NotFound);
        }

        [Test]
        [TestCaseSource(typeof(Constants.Extensions), "VideoExtensions")]
        public async Task RemoveContentAsync_VideoNotDeleted(string extension)
        {
            const ContentType type = ContentType.Video;
            const bool isSuccessed = false;
            var url = $"{Url}{extension}";
            _mockBlobStorageManager
                .Setup(repo => repo.RemoveContentAsync(It.IsAny<string>(), type))
                .Returns(Task.FromResult(isSuccessed));

            var actual = await _contentService.RemoveContentAsync(url, It.IsAny<ContentType>());

            Assert.IsTrue(actual.Result.Failed);
            Assert.AreEqual(actual.Result.Error.Value, Constants.Validation.Content.NotDeleted(url));
            Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.NotFound);
        }

        [Test]
        [TestCaseSource(typeof(Constants.Extensions), "ImageExtensions")]
        public async Task RemoveContentAsync_ExceptionThrownImage(string extension)
        {
            const ContentType type = ContentType.Image;
            var url = $"{Url}{extension}";
            _mockBlobStorageManager
                .Setup(repo => repo.RemoveContentAsync(It.IsAny<string>(), type))
                .Throws<Exception>();

            var actual = await _contentService.RemoveContentAsync(url, type);

            Assert.IsTrue(actual.Result.Failed);
            Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.InternalServerError);
        }

        [Test]
        [TestCaseSource(typeof(Constants.Extensions), "VideoExtensions")]
        public async Task RemoveContentAsync_ExceptionThrownVideo(string extension)
        {
            const ContentType type = ContentType.Video;
            var url = $"{Url}{extension}";
            _mockBlobStorageManager
                .Setup(repo => repo.RemoveContentAsync(It.IsAny<string>(), type))
                .Throws<Exception>();

            var actual = await _contentService.RemoveContentAsync(url, type);

            Assert.IsTrue(actual.Result.Failed);
            Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.InternalServerError);
        }
    }
}
