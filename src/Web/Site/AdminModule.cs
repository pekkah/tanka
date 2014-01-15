namespace Web.Site
{
    using System;
    using Documents;
    using Infrastructure;
    using Nancy;
    using Nancy.Security;
    using Raven.Client;
    using Tanka.Web.Infrastructure;

    public class AdminModule : NancyModule
    {
        public AdminModule(Func<IDocumentSession> sessionFactory)
            : base("/admin")
        {
            this.RequiresHttpsOrXProto();
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