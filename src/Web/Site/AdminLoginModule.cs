namespace Web.Site
{
    using Documents;
    using Infrastructure;
    using Nancy;
    using Nancy.Authentication.Forms;
    using Nancy.ModelBinding;
    using Nancy.Security;
    using Raven.Client;
    using HttpStatusCode = System.Net.HttpStatusCode;

    public class AdminLoginModule : NancyModule
    {
        public AdminLoginModule(ILoginService loginService, IDocumentStore store)
            : base("/admin")
        {
            this.RequiresHttps();

            Get["/login"] =
                parameters =>
                {
                    using (var session = store.OpenSession())
                    {
                        var site = session.GetSiteSettings();
                        return View["admin/login", new
                        {
                            Title = site.Title,
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
                                     return HttpStatusCode.Unauthorized;
                                 }

                                 return this.LoginAndRedirect(user.Identifier, fallbackRedirectUrl: "/admin");
                             };
        }

        public class LoginParameters
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }
    }
}