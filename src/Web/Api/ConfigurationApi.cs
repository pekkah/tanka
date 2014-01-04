namespace Api
{
    using Nancy;
    using Nancy.ModelBinding;
    using Nancy.Security;
    using Raven.Client;
    using Web.Documents;
    using Web.Infrastructure;
    using HttpStatusCode = System.Net.HttpStatusCode;

    public class ConfigurationApi : NancyModule
    {
        public ConfigurationApi(IDocumentSession documentSession)
            : base("api/configuration")
        {
            Get["/"] = parameters =>
                      {
                          var configuration = documentSession.GetConfiguration();

                          if (configuration == null)
                          {
                              configuration = new Configuration();
                          }

                          return configuration;
                      };

            Put["/"] = parameters =>
                       {
                           this.RequiresAuthentication();
                           this.RequiresClaims(new[] { SystemRoles.Administrators });

                           var configuration = this.Bind<Configuration>();
                           documentSession.StoreConfiguration(configuration);

                           return HttpStatusCode.OK;
                       };

        }
    }
}