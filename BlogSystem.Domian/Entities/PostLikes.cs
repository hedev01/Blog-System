using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Domian.Entities
{
    public class PostLikes
    {
        public int id { get; set; }
        public bool Like { get; set; }
        public Guid UserId { get; set; }

        public int PostId
        {
            get; set;
        }

        public DateTime CreateAt
        {
            get; set;
        }

        private PostLikes()
        {
        }

        public PostLikes(bool like, Guid userid, int postid)
        {
            Like = like;
            UserId = userid;
            PostId = postid;
            CreateAt = DateTime.UtcNow;
        }

        public void ChangeLike(bool like)
        {
            Like = like;
        }
    }
}
