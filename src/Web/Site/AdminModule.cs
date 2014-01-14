namespace Web.Site
{
    using System;
    using Documents;
    using Infrastructure;
    using Nancy;
    using Nancy.Security;
    using Raven.Client;

    public class AdminModule : NancyModule
    {
        public AdminModule(Func<IDocumentSession> sessionFactory)
            : base("/admin")
        {
            this.RequiresHttps();
            this.RequiresAuthentication();

            Get["/"] = parameters =>
            {
                using (IDocumentSession session = sessionFactory())
                {
                    SiteSettings site = session.GetSiteSettings();
                    return View["home", new {site.Title, site.SubTitle}];
                }
            };
        }
    }
}