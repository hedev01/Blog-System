using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Application.DTO.Auth
{
    public class RegisterResponse
    {
        public string UserName { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string Token { get; init; }
    }
}
