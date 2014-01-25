namespace Tanka.Web.Installer
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using BCrypt.Net;
    using Documents;
    using global::Nancy;
    using global::Nancy.ModelBinding;
    using Infrastructure;
    using Raven.Client;

    public class InstallerModule : NancyModule
    {
        public InstallerModule(Func<IDocumentSession> sessionFactory) : base("/installer")
        {
            this.RequiresHttpsOrXProto();

            Before.AddItemToStartOfPipeline(context =>
            {
                var key = Config.GetValue("tanka/installer/key");

                if (string.IsNullOrWhiteSpace(key))
                    return Response.AsText("Installer key not set in configuration");

                return null;
            });

            Get["/"] = parameters => View["home"];

            Post["/"] = parameters =>
            {
                var model = this.BindAndValidate<AdminDetailsModel>();

                if (!ModelValidationResult.IsValid)
                    return View["home", ModelValidationResult.Errors];

                using (IDocumentSession session = sessionFactory())
                {
                    var user = new User
                    {
                        UserName = model.Username,
                        Password = BCrypt.HashPassword(model.Password),
                        Roles = new []
                        {
                            SystemRoles.Administrators
                        }
                    };

                    session.Store(user);
                    session.SaveChanges();
                }

                return Response.AsRedirect("/admin");
            };
        }
    }

    public class AdminDetailsModel
    {
        [Required]
        public string Key { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}