namespace Api
{
    using System;
    using Nancy;
    using Nancy.ModelBinding;
    using Nancy.Security;
    using Raven.Client;
    using Tanka.Web.Infrastructure;
    using Web.Documents;
    using Web.Infrastructure;
    using HttpStatusCode = System.Net.HttpStatusCode;

    public class ConfigurationApi : NancyModule
    {
        public ConfigurationApi(Func<IDocumentSession> sessionFactory)
            : base("api/configuration")
        {
            this.RequiresHttpsOrXProto();
            this.RequiresAuthentication();
            this.RequiresClaims(new[] {SystemRoles.Administrators});

            Get["/"] = parameters =>
            {
                using (IDocumentSession session = sessionFactory())
                {
                    Configuration configuration = session.GetConfiguration();

                    if (configuration == null)
                    {
                        configuration = new Configuration();
                    }

                    return configuration;
                }
            };

            Put["/"] = parameters =>
            {
                using (IDocumentSession session = sessionFactory())
                {
                    var configuration = this.Bind<Configuration>();
                    session.StoreConfiguration(configuration);

                    return HttpStatusCode.OK;
                }
            };
        }
    }
}