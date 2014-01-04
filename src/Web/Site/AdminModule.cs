namespace Web.Site
{
    using Documents;
    using global::Web.Infrastructure.RavenDb;
    using Nancy;
    using Nancy.Security;
    using Raven.Client;

    public class AdminModule : NancyModule
    {
        public AdminModule(IDocumentSession session)
            : base("/admin")
        {
            this.RequiresAuthentication();
            Get["/"] = parameters =>
            {
                SiteSettings site = session.GetSiteSettings();
                return View["home", new {site.Title, site.SubTitle}];
            };
        }
    }
}