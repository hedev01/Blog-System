using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BlogSystem.Application.DTO.Features.Posts
{
    public record ListPostsRequest(
        int PageNumber = 1,
        int PageSize = 10,
        int AuthorId = 1,
        string SortOrder = "desc" // "asc" | "desc"
    );

    public class ListPostsQueryValidator : AbstractValidator<ListPostsRequest>
    {
        public ListPostsQueryValidator()
        {
            RuleFor(query => query.PageNumber).GreaterThan(0).WithMessage("شماره صفحه باید بزرگتر از 0 باشد");
            RuleFor(query => query.PageSize).GreaterThan(0).WithMessage("اندازه صفحه باید بزرگتر از 0 باشد");
            //RuleFor(query => query.AuthorId).GreaterThan(0).WithMessage("شماره نویسنده معتبر نمی باشد.");
            RuleFor(query => query.SortOrder).Must(s => s is "desc" or "asc")
                .WithMessage("فیلتر مرتب سازی فقط می‌تواند 'asc' یا 'desc' باشد.");
        }
    }
}
