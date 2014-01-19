namespace Tanka.Web.Api
{
    using System;
    using Documents;
    using global::Nancy;
    using global::Nancy.ModelBinding;
    using global::Nancy.Security;
    using Infrastructure;
    using Raven.Client;

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
                    session.SaveChanges();

                    return global::System.Net.HttpStatusCode.OK;
                }
            };
        }
    }
}