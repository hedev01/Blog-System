using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Domian.Entities
{
    public class Tag
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Slug { get; private set; }
        public int PostId { get; private set; }
        public ICollection<Post> Posts { get; private set; } = new List<Post>();

        //     private Tag() { }

        public Tag(string name)
        {

            Name = name.Trim();
            Slug = Name.ToLower().Replace(" ", "-");
        }
        public void Rename(string name)
        {
            Name = name.Trim();
            Slug = Name.ToLower().Replace(" ", "-");
        }
    }
}
