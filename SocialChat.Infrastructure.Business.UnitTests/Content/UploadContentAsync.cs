using Moq;
using NUnit.Framework;
using SocialChat.Domain.Core.Enums;
using SocialChat.Domain.Interfaces.Content;
using SocialChat.Infrastructure.Business.Content;
using SocialChat.Services.Interfaces.Content;
using System.Threading.Tasks;
using SocialChat.Domain.Core.Infrastructure;
using static SocialChat.Infrastructure.Business.UnitTests.Content.TestData.ContentTestData;
using System.Net;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace SocialChat.Infrastructure.Business.UnitTests.Content
{
    class UploadContentAsync
    {
        private IContentService _contentService;
        private readonly Mock<IBlobStorageManager> _mockBlobStorageManager;

        public UploadContentAsync()
        {
            _mockBlobStorageManager = new Mock<IBlobStorageManager>();
        }

        [SetUp]
        public void SetUp()
        {
            _contentService = new ContentService(_mockBlobStorageManager.Object);
        }


        [Test]
        [TestCaseSource(typeof(Constants.Extensions), "ImageExtensions")]
        public async Task UploadContentAsync_ImageValid(string extension)
        {
            var fileMock = new Mock<IFormFile>();
            var fileName = $"{new string(TestChar, CharsCount)}{extension}";
            const ContentType type = ContentType.Image;

            using (var ms = new MemoryStream())
            using (var sw = new StreamWriter(ms))
            {
                GenerateSampleFile(ms, sw, fileMock, fileName, new string(TestChar, CharsCount));
                _mockBlobStorageManager
                    .Setup(repo => repo.UploadFileAsync(fileMock.Object, type, fileName))
                    .Returns(Task.FromResult(new string(TestChar, CharsCount)));

                var actual = await _contentService.UploadContentAsync(fileMock.Object, type);

                Assert.IsTrue(actual.Result.Succeeded);
            }
        }

        [Test]
        [TestCaseSource(typeof(Constants.Extensions), "VideoExtensions")]
        public async Task UploadContentAsync_VideoValid(string extension)
        {
            var fileMock = new Mock<IFormFile>();
            var fileName = $"{new string(TestChar, CharsCount)}{extension}";
            const ContentType type = ContentType.Video;

            using (var ms = new MemoryStream())
            using (var sw = new StreamWriter(ms))
            {
                GenerateSampleFile(ms, sw, fileMock, fileName, new string(TestChar, CharsCount));
                _mockBlobStorageManager
                    .Setup(repo => repo.UploadFileAsync(fileMock.Object, type, fileName))
                    .Returns(Task.FromResult(new string(TestChar, CharsCount)));

                var actual = await _contentService.UploadContentAsync(fileMock.Object, type);

                Assert.IsTrue(actual.Result.Succeeded);
            }
        }

        [Test]
        [TestCaseSource(typeof(Constants.Extensions), "ImageExtensions")]
        public async Task UploadContentAsync_ImageEmtpy(string extension)
        {
            var fileMock = new Mock<IFormFile>();
            var fileName = $"{new string(TestChar, CharsCount)}{extension}";

            using (var ms = new MemoryStream())
            using (var sw = new StreamWriter(ms))
            {
                GenerateSampleFile(ms, sw, fileMock, fileName, string.Empty);
                _mockBlobStorageManager
                    .Setup(repo => repo.UploadFileAsync(fileMock.Object, It.IsAny<ContentType>(), fileName))
                    .Returns(Task.FromResult(new string(TestChar, CharsCount)));

                var actual = await _contentService.UploadContentAsync(fileMock.Object, It.IsAny<ContentType>());

                Assert.IsTrue(actual.Result.Failed);
                Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.UnprocessableEntity);
            }
        }

        [Test]
        [TestCaseSource(typeof(Constants.Extensions), "VideoExtensions")]
        public async Task UploadContentAsync_VideoEmtpy(string extension)
        {
            var fileMock = new Mock<IFormFile>();
            var fileName = $"{new string(TestChar, CharsCount)}{extension}";
            const ContentType type = ContentType.Video;

            using (var ms = new MemoryStream())
            using (var sw = new StreamWriter(ms))
            {
                GenerateSampleFile(ms, sw, fileMock, fileName, string.Empty);
                _mockBlobStorageManager
                    .Setup(repo => repo.UploadFileAsync(fileMock.Object, type, fileName))
                    .Returns(Task.FromResult(new string(TestChar, CharsCount)));

                var actual = await _contentService.UploadContentAsync(fileMock.Object, type);

                Assert.IsTrue(actual.Result.Failed);
                Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.UnprocessableEntity);
            }
        }

        [Test]
        public async Task UploadContentAsync_FileNull()
        {
            var actual = await _contentService.UploadContentAsync(null, It.IsAny<ContentType>());

            Assert.IsTrue(actual.Result.Failed);
            Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.UnprocessableEntity);
        }

        [Test]
        [TestCase(TestIncorrectExtension)]
        public async Task UploadContentAsync_UnsupportedExtension(string extension)
        {
            var fileMock = new Mock<IFormFile>();
            var fileName = $"{new string(TestChar, CharsCount)}{extension}";
            const ContentType type = ContentType.Video;

            using (var ms = new MemoryStream())
            using (var sw = new StreamWriter(ms))
            {
                GenerateSampleFile(ms, sw, fileMock, fileName, new string(TestChar, CharsCount));

                var actual = await _contentService.UploadContentAsync(fileMock.Object, type);

                Assert.IsTrue(actual.Result.Failed);
                Assert.AreEqual(actual.Result.Error.Key, HttpStatusCode.UnsupportedMediaType);
            }
        }
    }
}
