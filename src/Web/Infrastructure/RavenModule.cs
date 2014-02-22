namespace Tanka.Web.Infrastructure
{
    using System;
    using Autofac;
    using Raven.Client;
    using Raven.Client.Document;

    public class RavenModule : Module
    {
        private static readonly Lazy<IDocumentStore> DocumentStoreLazy = new Lazy<IDocumentStore>(GetDocumentStore);

        public static Func<IDocumentStore> CreateDocumentStore = () =>
        {
            var store = new DocumentStore
            {
                ConnectionStringName = "RavenDB",
                Conventions =
                {
                    FindClrTypeName = type => type.FullName,
                    FindClrType = (s, o, metadata) => metadata.Value<string>("Raven-Clr-Type")
                }
            };

            store.Initialize();

            return store;
        };

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(DocumentStoreLazy.Value)
                .As<IDocumentStore>()
                .SingleInstance();

            builder.Register(context => context.Resolve<IDocumentStore>().OpenSession())
                .As<IDocumentSession>()
                .InstancePerDependency();
        }

        private static IDocumentStore GetDocumentStore()
        {
            return CreateDocumentStore();
        }
    }
}