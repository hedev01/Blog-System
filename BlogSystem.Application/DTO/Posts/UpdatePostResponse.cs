using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogSystem.Domian.Entities;

namespace BlogSystem.Application.DTO
{
    public class UpdatePostResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<string> Tags { get; set; } = new();
    }
}
