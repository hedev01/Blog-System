using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BlogSystem.Application.DTO.Auth
{
    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }

    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(request => request.Username).NotEmpty().WithMessage("نام کاربری باید وارد شود.");
            RuleFor(request => request.Password).NotEmpty().WithMessage("رمز عبور باید وارد شود. ").MinimumLength(8)
                .WithMessage("رمز عبور باید حداقل شامل 8 کاراکتر باشد.");
            RuleFor(request => request.Email)
                .NotEmpty().WithMessage("ایمیل باید وارد شود.")
                .EmailAddress().WithMessage("فرمت ایمیل نامعتبر است.");

            RuleFor(request => request.Password)
                .NotEmpty().WithMessage("رمز عبور باید وارد شود.")
                .MinimumLength(8).WithMessage("رمز عبور باید حداقل شامل 8 کاراکتر باشد.")
                .Matches("[A-Z]").WithMessage("رمز عبور باید حداقل شامل یک حرف بزرگ انگلیسی باشد.")
                .Matches("[a-z]").WithMessage("رمز عبور باید حداقل شامل یک حرف کوچک انگلیسی باشد.")
                .Matches("[0-9]").WithMessage("رمز عبور باید حداقل شامل یک عدد باشد.")
                .Matches("[^a-zA-Z0-9]").WithMessage("رمز عبور باید حداقل شامل یک علامت (مانند @, #, !, ?) باشد.");
            RuleFor(request => request.FirstName).NotEmpty().WithMessage("نام باید وارد شود.");
            RuleFor(request => request.LastName).NotEmpty().WithMessage("نام خانوادگی باید وارد شود.");

        }
    }
}
