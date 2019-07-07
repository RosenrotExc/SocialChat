using Microsoft.AspNetCore.Http;
using Moq;
using System.IO;

namespace SocialChat.Infrastructure.Business.UnitTests.Content.TestData
{
    static class ContentTestData
    {
        public const char TestChar = 'c';
        public const int CharsCount = 1;
        public const string TestIncorrectExtension = ".test";

        public static IFormFile GenerateSampleFile(
            MemoryStream ms, 
            StreamWriter sw, 
            Mock<IFormFile> fileMock, 
            string fileName,
            string content)
        {
            sw.Write(content);
            sw.Flush();
            ms.Position = 0;

            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);

            return fileMock.Object;
        }

        public class RemoveContentAsync_TestData
        {
            public string TestUrl = "https://test.test/guid/content";
        }
    }
}
