using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Application.DTO
{
    public class CreatePostResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<string> Tags { get; set; } = new();

   
    }
}
