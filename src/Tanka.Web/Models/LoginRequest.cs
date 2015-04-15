namespace Tanka.Web.Models
{
    using FluentValidation;

    public class LoginRequest
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }

    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(p => p.UserName).Length(3, 200);
            RuleFor(p => p.Password).Length(6, 200);
        }
    }
}