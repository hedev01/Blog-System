using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogSystem.Domian.Entities;

namespace BlogSystem.Application.DTO.Features.Posts
{
    public class UpdatePostRequest
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string? CoverImageUrl { get; set; }
        public string Status { get; set; }
        public List<string> Tags { get; set; } = new();
        public int AuthorId { get; set; }
    }
}
