using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace BlogSystem.Application.DTO.User
{
    public record LoginRequest(string username , string password);

    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(request => request.username).NotEmpty().WithMessage("نام کاربری باید وارد شود.");
            RuleFor(request => request.password).NotEmpty().WithMessage("رمز عبور باید وارد شود. ").MinimumLength(8)
                .WithMessage("رمز عبور باید حداقل شامل 8 کاراکتر باشد.");
        }
    }
}
