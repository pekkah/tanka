namespace Tanka.Web.Api
{
    using System;
    using Documents;
    using global::Nancy;
    using global::Nancy.ModelBinding;
    using global::Nancy.Security;
    using Infrastructure;
    using Raven.Client;

    public class SiteSettingsApi : NancyModule
    {
        public SiteSettingsApi(Func<IDocumentSession> sessionFactory) : base("api/settings")
        {
            this.RequiresHttpsOrXProto();
            this.RequiresAuthentication();
            this.RequiresClaims(new[] {SystemRoles.Administrators});

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
                using (IDocumentSession session = sessionFactory())
                {
                    this.RequiresAuthentication();
                    this.RequiresClaims(new[] {SystemRoles.Administrators});
                    var settings = this.Bind<SiteSettings>();
                    session.StoreSiteSettings(settings);
                    session.SaveChanges();

                    return global::System.Net.HttpStatusCode.OK;
                }
            };
        }
    }
}