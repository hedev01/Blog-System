using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogSystem.Domian.Entities;

namespace BlogSystem.Application.DTO
{
    public class GetPostResponse
    {
        public PagedResult<Post> Result { get; set; }

        public GetPostResponse(PagedResult<Post> result)
        {
            Result = result;
        }
    }
}
