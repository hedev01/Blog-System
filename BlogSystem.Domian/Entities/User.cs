using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Domian.Entities
{
    public class User
    {

        public int Id { get; set; }


        public string Username { get; set; }
        public string Email { get; set; }


        public string PasswordHash { get; set; }


        public string? FirstName { get; set; }
        public string? LastName { get; set; }


        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }


        private User()
        {
        }

        public User(string username, string email, string password, string firstname, string lastname)
        {
            Username = username;
            Email = email;
            PasswordHash = password;
            FirstName = firstname;
            LastName = lastname;
            IsActive = true;
            CreatedAt = DateTime.Now;
            LastLoginAt = CreatedAt;
        }

    }
}
