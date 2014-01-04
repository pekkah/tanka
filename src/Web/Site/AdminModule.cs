namespace Web.Site
{
    using Documents;
    using Infrastructure;
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