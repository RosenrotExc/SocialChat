using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Security.Authentication;

namespace SocialChat.Infrastructure.Data
{
    public class CloudContext
    {
        private MongoClient _mongoClient;
        private IConfiguration _configuration;

        public CloudContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public MongoClient GetMongoDBClient()
        {
            if (_mongoClient != null)
            {
                return _mongoClient;
            }

            var settings = MongoClientSettings.FromUrl(new MongoUrl(_configuration["CosmosDBConnectionString"]));
            settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            return new MongoClient(settings);
        }
    }
}
