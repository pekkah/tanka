namespace Tanka.Web.Api
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using BCrypt.Net;
    using global::Nancy;
    using global::Nancy.ModelBinding;
    using global::Nancy.Security;
    using global::Web.Documents;
    using Raven.Client;

    public class ChangePasswordRequest
    {
        [Required]
        public string Password { get; set; }
    }

    public class UserModule : NancyModule
    {
        public UserModule(Func<IDocumentSession> sessionFactory)
            : base("/api/users/current")
        {
            this.RequiresHttps();
            this.RequiresAuthentication();

            Post["/password"] = r =>
            {
                var request = this.BindAndValidate<ChangePasswordRequest>();

                if (!ModelValidationResult.IsValid)
                    return HttpStatusCode.BadRequest;

                string password = request.Password;
                string userName = Context.CurrentUser.UserName;

                using (IDocumentSession session = sessionFactory())
                {
                    User user = session.Query<User>().Single(u => u.UserName == userName);
                    user.Password = BCrypt.HashPassword(password);
                    session.SaveChanges();

                    return HttpStatusCode.OK;
                }
            };
        }
    }
}