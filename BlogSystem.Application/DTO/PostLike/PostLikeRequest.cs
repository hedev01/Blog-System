using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Application.DTO.PostLike
{
    public record PostLikeRequest(bool Like , Guid UserId , int PostId);
}
