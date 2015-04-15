namespace Tanka.Web.Models
{
    using FluentValidation;

    public class ChangePasswordRequest
    {
        public string Password { get; set; }
    }

    public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordRequestValidator()
        {
            RuleFor(p => p.Password).NotEmpty();
        }
    }
}