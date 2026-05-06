using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace BlogSystem.Application.DTO
{
    public class CreatePostRequest
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string? CoverImageUrl { get; set; }
        public string Status { get; set; }
        public List<string> Tags { get; set; } = new();
        public int AuthorId { get; set; }
    }

    public class CreatePostRequestValidator : AbstractValidator<CreatePostRequest>
    {
        public CreatePostRequestValidator()
        {
            RuleFor(request => request.Title).NotEmpty().WithMessage("عنوان الزامی می باشد.").MaximumLength(150);
            RuleFor(request => request.Content).NotEmpty().WithMessage("محتوا الزامی می باشد.").MinimumLength(20).WithMessage("محتوا باید حداقل شامل 20 کاراکتر باشد.");
            RuleFor(request => request.Status).Must(s => s is "published" or "draft")
                .WithMessage("وضعیت انتشار صحیح نمی باشد.");
            RuleFor(request => request.Tags.Count).LessThanOrEqualTo(10).WithMessage("حداکثر 10 تگ مجاز می باشد");
            RuleFor(x => x.CoverImageUrl)
                .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
                .When(x => !string.IsNullOrWhiteSpace(x.CoverImageUrl))
                .WithMessage("تصویر نامعتبر است.");
            


        }
    }

}
