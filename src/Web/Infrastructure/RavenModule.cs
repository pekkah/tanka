namespace Tanka.Web.Infrastructure
{
    using Autofac;
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
                .InstancePerDependency();
        }

        public static IDocumentStore GetDocumentStore(IComponentContext context)
        {
            var store = new DocumentStore
            {
                ConnectionStringName = "RavenDb",
                Conventions =
                {
                    FindClrTypeName = type => type.FullName,
                    FindClrType = (s, o, metadata) => metadata.Value<string>("Raven-Clr-Type")
                }
            };

            store.Initialize();

            return store;
        }
    }
}