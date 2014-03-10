namespace Tanka.Web.Models
{
    using System.Collections.Generic;
    using FluentValidation;

    public class CreateUserRequest
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }

    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator()
        {
            RuleFor(p => p.UserName).NotNull().Length(3, 100);
            RuleFor(p => p.Password).NotNull().Length(6, 200);
        }
    }
}