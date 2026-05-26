using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BlogSystem.Application.DTO.Comment
{
    public record CommentRequest(int postId , string comment , Guid UserId);

}
