namespace Tanka.Web.Site
{
    using System;
    using Documents;
    using global::Nancy;
    using global::Nancy.Authentication.Forms;
    using global::Nancy.ModelBinding;
    using Infrastructure;
    using Raven.Client;

    public class AdminLoginModule : NancyModule
    {
        public AdminLoginModule(ILoginService loginService, IDocumentStore store)
            : base("/admin")
        {
            this.RequiresInstallerDisabled(() => store.OpenSession());
            this.RequiresHttpsOrXProto();

            Get["/login"] =
                parameters =>
                {
                    using (IDocumentSession session = store.OpenSession())
                    {
                        SiteSettings site = session.GetSiteSettings();

                        if (site == null)
                        {
                            site = new SiteSettings()
                            {
                                Title = "Admin",
                                SubTitle = "Go to Site -> Settings"
                            };
                        }

                        return View["admin/login", new
                        {
                            site.Title,
                            SubTitle = "Login"
                        }];
                    }
                };

            Get["/logout"] = parameters =>
            {
                // Called when the user clicks the sign out button in the application. Should
                // perform one of the Logout actions (see below)

                return View["admin/logout"];
            };

            Post["/login"] = parameters =>
            {
                // Called when the user submits the contents of the login form. Should
                // validate the user based on the posted form data, and perform one of the
                // Login actions (see below)
                var loginParameters = this.Bind<LoginParameters>();

                User user;
                if (!loginService.Login(loginParameters.UserName, loginParameters.Password, out user))
                {
                    return global::System.Net.HttpStatusCode.Unauthorized;
                }

                return this.LoginAndRedirect(
                    user.Identifier,
                    fallbackRedirectUrl: "/admin",
                    cookieExpiry: DateTime.Now.AddHours(1));
            };
        }

        public class LoginParameters
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }
    }
}