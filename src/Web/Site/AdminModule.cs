namespace Tanka.Web.Site
{
    using System;
    using Documents;
    using global::Nancy;
    using global::Nancy.Security;
    using Infrastructure;
    using Raven.Client;

    public class AdminModule : NancyModule
    {
        public AdminModule(Func<IDocumentSession> sessionFactory)
            : base("/admin")
        {
            this.RequiresInstallerDisabled(sessionFactory);
            this.RequiresHttpsOrXProto();
            this.RequiresAuthentication();

            Get["/"] = parameters =>
            {
                using (IDocumentSession session = sessionFactory())
                {
                    SiteSettings site = session.GetSiteSettings();

                    if (site == null)
                        site = new SiteSettings()
                        {
                            SubTitle = "Go to site -> settings"
                        };

                    return View["home", new {site.Title, site.SubTitle}];
                }
            };
        }
    }
}