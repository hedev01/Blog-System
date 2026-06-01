using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Domian.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }

        public string Token { get; set; }

        public Guid UserId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ExpireAt { get; set; }

        public bool IsRevoked { get; set; }

        public bool IsActive => !IsRevoked && ExpireAt > DateTime.UtcNow;
    }
}
