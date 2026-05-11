using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogSystem.Domian.Entities;

namespace BlogSystem.Application.DTO.Features.Posts
{
    public class ListPostResponse
    {
        //    p.Title, p.Content, p.CoverImageUrl, p.Status, p.AuthorId
        public int Id { get; init; }
        public string Title { get; init; }
        public string Content { get; init; }
        public string CoverImageUrl { get; init; }
        public string Status { get; init; }
        public int AuthorId { get; init; }
        public List<string> TagName
        {
            get
            ;
            init;
        }
    }
}
