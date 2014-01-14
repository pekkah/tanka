namespace Api
{
    using System;
    using Nancy;
    using Nancy.ModelBinding;
    using Nancy.Security;
    using Raven.Client;
    using Web.Documents;
    using Web.Infrastructure;
    using HttpStatusCode = System.Net.HttpStatusCode;

    public class SiteSettingsApi : NancyModule
    {
        public SiteSettingsApi(Func<IDocumentSession> sessionFactory) : base("api/settings")
        {
            Get["/"] = parameters =>
            {
                using (IDocumentSession session = sessionFactory())
                {
                    SiteSettings settings = session.GetSiteSettings();

                    if (settings == null)
                    {
                        settings = new SiteSettings();
                    }

                    return settings;
                }
            };

            Put["/"] = parameters =>
            {
                this.RequiresHttps();

                using (IDocumentSession session = sessionFactory())
                {
                    this.RequiresAuthentication();
                    this.RequiresClaims(new[] {SystemRoles.Administrators});
                    var settings = this.Bind<SiteSettings>();
                    session.StoreSiteSettings(settings);

                    return HttpStatusCode.OK;
                }
            };
        }
    }
}