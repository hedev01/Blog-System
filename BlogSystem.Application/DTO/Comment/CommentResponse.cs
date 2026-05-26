using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Application.DTO.Comment
{
    public class CommentResponse
    {
        public int Id { get; init; }
        public string Comment { get; init; }
        public int PostId { get; init; }

    }
}
