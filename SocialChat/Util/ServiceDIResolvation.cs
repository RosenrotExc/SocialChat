using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using SocialChat.Domain.Interfaces.Content;
using SocialChat.Domain.Interfaces.Users;
using SocialChat.Infrastructure.Business.Blogs;
using SocialChat.Infrastructure.Business.Content;
using SocialChat.Infrastructure.Business.Users;
using SocialChat.Infrastructure.Data;
using SocialChat.Infrastructure.Data.Content;
using SocialChat.Infrastructure.Data.Users;
using SocialChat.Services.Interfaces.Blogs;
using SocialChat.Services.Interfaces.Content;
using SocialChat.Services.Interfaces.Users;

namespace SocialChat.Util
{
    public static class ServiceDIResolvation
    {
        public static IServiceCollection RegisterDI(this IServiceCollection services, IConfiguration configuration) =>
            services.AddSingleton<CloudBlobClient>(_ => CloudStorageAccount
                    .Parse(configuration["BlobStorageConnectionString"])
                    .CreateCloudBlobClient())
                .AddTransient<IDbConnection, SqlConnection>(provider => new SqlConnection(configuration["ConnectionString"]))
                .AddSingleton(new CloudContext(configuration))
                .AddScoped<IUserService, UserService>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IBlogService, BlogService>()
                .AddScoped<IBlobStorageManager, BlobStorageManager>()
                .AddScoped<IContentService, ContentService>();
    }
}
