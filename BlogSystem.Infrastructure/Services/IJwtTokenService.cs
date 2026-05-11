using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Infrastructure.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(string userName);
    }
}
