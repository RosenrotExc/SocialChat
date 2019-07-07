using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SocialChat.Domain.Core.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace SocialChat.Domain.Core.Models.Blogs
{
    public class Blog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = Constants.Validation.Blogs.TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(1000, ErrorMessage = Constants.Validation.Blogs.TextContentMaxLength)]
        public string Text { get; set; }

        [Required]
        public int AuthorId { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
