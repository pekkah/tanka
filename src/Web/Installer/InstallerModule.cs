﻿namespace Tanka.Web.Installer
{
    using System;
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
                using (var session = sessionFactory())
                {
                    var settings = session.GetSiteSettings();

                    if (!settings.IsInstallerEnabled)
                        return Response.AsRedirect("/admin");
                }

                var key = Config.GetValue("tanka/installer/key");

                if (string.IsNullOrWhiteSpace(key) || key == "null")
                    return Response.AsText("Installer key not set in configuration");

                return null;
            });

            Get["/"] = parameters => View["home"];

            Post["/"] = parameters =>
            {
                var model = this.BindAndValidate<AdminDetailsModel>();

                if (!ModelValidationResult.IsValid)
                    return View["home", ModelValidationResult.Errors];

                var key = Config.GetValue("tanka/installer/key");

                if (key != model.Key)
                {
                    return View["home", ModelValidationResult.Errors];
                }

                using (IDocumentSession session = sessionFactory())
                {
                    var user = new User
                    {
                        UserName = model.Username,
                        Identifier = Guid.NewGuid(),
                        Password = BCrypt.HashPassword(model.Password),
                        Roles = new[]
                        {
                            SystemRoles.Administrators
                        }
                    };

                    session.Store(user);

                    var settings = session.GetSiteSettings();
                    settings.IsInstallerEnabled = false;
                    session.StoreSiteSettings(settings);

                    session.SaveChanges();
                }

                return Response.AsRedirect("/admin");
            };
        }
    }
}