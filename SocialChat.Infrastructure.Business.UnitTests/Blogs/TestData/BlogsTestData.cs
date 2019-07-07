using Microsoft.AspNetCore.Mvc.ModelBinding;
using SocialChat.Domain.Core.Models.Blogs;
using System;
using System.Collections.Generic;

namespace SocialChat.Infrastructure.Business.UnitTests.Blogs.TestData
{
    static class BlogsTestData
    {
        private const char TestChar = '-';
        private const int CharsCount = 10;

        public class CreateBlogAsync_TestData
        {
            public List<Blog> Blogs = new List<Blog>
            {
                new Blog
                {
                    AuthorId = 1,
                    CreateDate = DateTime.Now,
                    Id = new string(TestChar, CharsCount),
                    Text = new string(TestChar, CharsCount),
                    Title = new string(TestChar, CharsCount)
                }
            };

            public ModelStateDictionary ModelState = new ModelStateDictionary();
        }

        public class DeleteBlogAsync_TestData
        {
            public string Id = new string(TestChar, CharsCount);
        }

        public class GetBlog_TestData
        {
            public List<Blog> Blogs = new List<Blog>
            {
                new Blog
                {
                    AuthorId = 1,
                    CreateDate = DateTime.Now,
                    Id = new string(TestChar, CharsCount),
                    Text = new string(TestChar, CharsCount),
                    Title = new string(TestChar, CharsCount)
                }
            };

            public string Id = new string(TestChar, CharsCount);
        }

        public class GetBlogs_TestData
        {
            public List<Blog> Blogs = new List<Blog>
            {
                new Blog
                {
                    AuthorId = 1,
                    CreateDate = DateTime.Now,
                    Id = new string(TestChar, CharsCount),
                    Text = new string(TestChar, CharsCount),
                    Title = new string(TestChar, CharsCount)
                }
            };

            public List<string> Ids = new List<string>
            {
                new string(TestChar, CharsCount)
            };
        }

        public class UpdateBlogAsync_TestData
        {
            public List<Blog> Blogs = new List<Blog>
            {
                new Blog
                {
                    AuthorId = 1,
                    CreateDate = DateTime.Now,
                    Id = new string(TestChar, CharsCount),
                    Text = new string(TestChar, CharsCount),
                    Title = new string(TestChar, CharsCount)
                }
            };

            public ModelStateDictionary ModelState = new ModelStateDictionary();
        }
    }
}
