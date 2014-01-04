namespace Api
{
    using Nancy;
    using Nancy.ModelBinding;
    using Nancy.Security;
    using Raven.Client;
    using Web.Documents;
    using Web.Infrastructure;
    using HttpStatusCode = System.Net.HttpStatusCode;

    public class SiteSettingsApi : NancyModule
    {
        public SiteSettingsApi(IDocumentSession documentSession) : base("api/settings")
        {
            Get["/"] = parameters =>
                       {
                           var settings = documentSession.GetSiteSettings();

                           if (settings == null)
                           {
                               settings = new SiteSettings();
                           }

                           return settings;
                       };

            Put["/"] = parameters =>
                       {
                           this.RequiresAuthentication();
                           this.RequiresClaims(new[] { SystemRoles.Administrators });
                           var settings = this.Bind<SiteSettings>();
                           documentSession.StoreSiteSettings(settings);

                           return HttpStatusCode.OK;
                       };
        }

    }
}