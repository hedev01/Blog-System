using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogSystem.Domian.Enums;

namespace BlogSystem.Domian.Entities
{
    public class User
    {

        public int Id { get; set; }

        public Guid PublicId { get; set; }


        public string Username { get; set; }
        public string Email { get; set; }


        public string PasswordHash { get; set; }


        public string? FirstName { get; set; }
        public string? LastName { get; set; }


        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }

        public Role Role { get; set; }


        private User()
        {
        }

        public User(string username, string email, string password, string firstname, string lastname , Role role)
        {
            PublicId = Guid.NewGuid();
            Username = username;
            Email = email;
            PasswordHash = password;
            FirstName = firstname;
            LastName = lastname;
            IsActive = true;
            CreatedAt = DateTime.Now;
            LastLoginAt = CreatedAt;
            Role = role;
        }

    }
}
