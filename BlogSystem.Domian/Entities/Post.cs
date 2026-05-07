using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;

namespace BlogSystem.Domian.Entities
{
    public class Post
    {


        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Slug { get; private set; }
        public string Content { get; private set; }
        public string? CoverImageUrl { get; private set; }
        public string Status { get; private set; }
        public int AuthorId { get; private set; }

        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public DateTime? PublishedAt { get; private set; }
        public ICollection<Tag> Tags { get; private set; } = new List<Tag>();
        public Post(string title, string content, string? coverImageUrl, string status, int authorId)
        {


            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty");

            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Content cannot be empty");

            Title = title;
            Slug = GenerateSlug(title);
            Content = content;
            CoverImageUrl = coverImageUrl;
            Status = status;
            AuthorId = authorId;

            CreatedAt = DateTime.UtcNow;
            UpdatedAt = CreatedAt;

            if (Status == "published")
                PublishedAt = CreatedAt;
        }

        public void Edit(string title, string content, string? coverImageUrl, string status, int authorId)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty");

            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Content cannot be empty");

            Title = title;
            Slug = GenerateSlug(title);
            Content = content;
            CoverImageUrl = coverImageUrl;
            Status = status;
            AuthorId = authorId;

            
            UpdatedAt = DateTime.UtcNow;

            if (Status == "published")
                PublishedAt = CreatedAt;
        }

        private string GenerateSlug(string title)
        {
            return title.Trim().ToLower().Replace(" ", "-");
        }
    }
}

public enum Status
{
    draft,
    published
}
