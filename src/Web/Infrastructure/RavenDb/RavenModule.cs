namespace Web.Infrastructure.RavenDb
{
    using Autofac;
    using Configuration;
    using Raven.Client;
    using Raven.Client.Document;

    public class RavenModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(GetDocumentStore)
                   .As<IDocumentStore>()
                   .SingleInstance();

            builder.Register(context => context.Resolve<IDocumentStore>().OpenSession())
                   .As<IDocumentSession>()
                   .InstancePerLifetimeScope();
        }

        public static IDocumentStore GetDocumentStore(IComponentContext context)
        {
            var configuration = context.Resolve<WebConfiguration>();

            var store = new DocumentStore
                        {
                            ConnectionStringName = "RavenDb"
                        };

            store.Conventions.FindClrTypeName = type => type.FullName;
            store.Conventions.FindClrType = (s, o, metadata) => metadata.Value<string>("Raven-Clr-Type");

            store.Initialize();

            return store;
        }
    }
}