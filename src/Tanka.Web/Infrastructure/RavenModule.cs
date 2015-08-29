﻿namespace Tanka.Web.Infrastructure
{
    using System;
    using Raven.Client;
    using Raven.Client.Document;
    using Microsoft.Framework.DependencyInjection;

    public static class RavenModule
    {
        public static Func<string, IDocumentStore> CreateDocumentStore = (connectionString) =>
        {
            var store = new DocumentStore
            {
                Conventions =
                {
                    FindClrTypeName = type => type.FullName,
                    FindClrType = (s, o, metadata) => metadata.Value<string>("Raven-Clr-Type")
                }
            };

            store.ParseConnectionString(connectionString);
            store.Initialize();

            return store;
        };

        public static void AddRaven(this IServiceCollection services, string connectionString)
        {
            services.AddSingleton<IDocumentStore>(provider => CreateDocumentStore(connectionString));
            services.AddTransient<IDocumentSession>(
                provider => provider.GetRequiredService<IDocumentStore>().OpenSession());
        }
    }
}