using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Domian.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public Post Post { get; set; }

        private Comment()
        {
            
        }

        public Comment(int postid , Guid userid , string content)
        {
            PostId = postid;
            UserId = userid;
            Content = content;
            Created = DateTime.Now;
        }
    }
}
