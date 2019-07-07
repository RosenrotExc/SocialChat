using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using SocialChat.Domain.Core.Models.Blogs;
using SocialChat.Domain.Interfaces.Blogs;
using System.Linq;

namespace SocialChat.Infrastructure.Data.Blogs
{
    public class BlogRepository : IBlogRepository
    {
        private readonly CloudContext _cloudContext;
        private readonly IConfiguration _configuration;

        public BlogRepository(CloudContext cloudContext, IConfiguration configuration)
        {
            _cloudContext = cloudContext;
            _configuration = configuration;
        }

        private IMongoCollection<Blog> GetMongoCollection =>
            _cloudContext
            .GetMongoDBClient()
            .GetDatabase(_configuration["DbName"])
            .GetCollection<Blog>(_configuration["DbBlogCollectionName"]);

        public async Task CreateBlogAsync(Blog blog) =>
            await GetMongoCollection
            .InsertOneAsync(blog);

        public async Task<long> DeleteBlogAsync(string id)
        {
            var result = await GetMongoCollection.DeleteOneAsync(Builders<Blog>.Filter.Eq("_id", ObjectId.Parse(id)));
            return result.DeletedCount;
        }

        public Blog GetBlog(string id) =>
            GetMongoCollection
            .AsQueryable()
            .FirstOrDefault(blog => blog.Id == id);

        public IEnumerable<Blog> GetBlogs(IEnumerable<string> ids) =>
            GetMongoCollection
            .Find(Builders<Blog>.Filter.In(x => x.Id, ids))
            .ToEnumerable();

        public async Task UpdateBlogAsync(Blog blog) =>
            await GetMongoCollection
            .ReplaceOneAsync(Builders<Blog>.Filter.Eq("_id", ObjectId.Parse(blog.Id)), blog);

        public bool CheckForExistedBlog(string title) =>
            GetMongoCollection
            .AsQueryable()
            .Any(blog => blog.Title == title);
    }
}
