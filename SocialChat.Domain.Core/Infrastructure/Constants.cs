using SocialChat.Domain.Core.Enums;

namespace SocialChat.Domain.Core.Infrastructure
{
    public static class Constants
    {
        public static class Extensions
        {
            public static string[] GetExtensions(ContentType type)
            {
                switch (type)
                {
                    case ContentType.Video: return VideoExtensions;
                    case ContentType.Image: return ImageExtensions;
                    default: return new string[0];
                }
            }

            public static readonly string[] ImageExtensions = new string[] {
                ".jpe",
                ".jpg",
                ".jpeg",
                ".gif",
                ".png",
                ".bmp",
                ".ico",
                ".svg",
                ".svgz",
                ".tif",
                ".tiff",
                ".ai",
                ".drw",
                ".pct",
                ".psp",
                ".xcf",
                ".psd",
                ".raw"
            };

            public static readonly string[] VideoExtensions = new string[] {
                ".mp4"
            };
        }

        public static class Validation
        {
            public static class Blogs
            {
                public static string IncorrectId() =>
                    "Incorrect blog id!";

                public static string BlogNotFound(string id) =>
                    $"Blog with id {id} not found!";

                public static string BlogsNotFound() =>
                    "No blogs in database!";

                public static string SameBlogTitleExists() =>
                    "Same blog title already exists!";

                public static string NoIdsRecieved() =>
                    "Ids collection is empty!";

                public const string TitleMaxLength = "Sorry, but max length of title is 30!";
                public const string TextContentMaxLength = "Sorry, but max length of text content is 1000!";
            }

            public static class Users
            {
                public static string IncorrectId() =>
                    "Incorrect user id!";

                public static string UserNotFound(int id) =>
                    $"User with id {id} not found!";

                public static string UsersNotFound() =>
                    "No users in database!";

                public static string SameUserExists() =>
                    "Same user already exists!";

                public const string FirstNameMaxLength = "Sorry, but max length of first name is 30!";
                public const string LastNameMaxLength = "Sorry, but max length of last name is 30!";
                public const string EmailMaxLength = "Sorry, but max length of email is 30!";
                public const string EmailError = "Check whether your email is correct!";
            }

            public static class Content
            {
                public static string InvalidFile(ContentType type) => 
                    $"Invalid {type} has been recieved!";

                public static string WrongFileExtension(ContentType type) => 
                    $"Wrong {type} extension!";

                public static string WrongUrl(string url) => 
                    $"Wrong url: {url}";

                public static string NotDeleted(string url) => 
                    $"Seems that {url} already deleted, or never existed!";

                public const int MaxAllowedFileSize = 50_000_000;
            }

            public static class CommonErrors
            {
                public static string ServerError(string message) =>
                    $"Seems that something went wrong with server: {message}";

                public static string SQLError(string message) =>
                    $"Seems that something went wrong with database: {message}";

                public static string BlobStorageError(string message) =>
                    $"Seems that something went wrong with content storage: {message}";

                public static string CosmosError(string message) =>
                    $"Seems that something went wrong with CosmosDB: {message}";

                public static string IncorrectDataProvided() =>
                    "Incorrect data provided!";
            }
        }
    }
}
