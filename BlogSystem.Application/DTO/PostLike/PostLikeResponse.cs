using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Application.DTO.PostLike
{
    public class PostLikeResponse
    {
        public int Id { get; init; }
        public bool Like { get; init; }
        public Guid UserId { get; set; }

        public int PostId
        {
            get; set;
        }
    }
}
