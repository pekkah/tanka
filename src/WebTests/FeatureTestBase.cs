﻿namespace Tanka.WebTests
{
    using System;
    using Nancy;
    using Nancy.Authentication.Forms;
    using Nancy.Testing;
    using Raven.Client;
    using Web.Documents;
    using Web.Infrastructure;

    public abstract class FeatureTestBase : DatabaseTestBase
    {
        protected Lazy<IDocumentStore> StoreFactory = new Lazy<IDocumentStore>(NewDocumentStore);

        protected FeatureTestBase()
        {
            FormsConfig = new FormsAuthenticationConfiguration
            {
                RedirectUrl = "~/admin/login",
                UserMapper = new FormsAuthenticationUserMapper(Store.OpenSession)
            };
        }

        protected FormsAuthenticationConfiguration FormsConfig { get; set; }

        protected IDocumentStore Store
        {
            get { return StoreFactory.Value; }
        }

        protected User NewUser(string userName, params string[] roles)
        {
            using (IDocumentSession session = Store.OpenSession())
            {
                var user = new User
                {
                    Identifier = Guid.NewGuid(),
                    UserName = userName,
                    Roles = roles
                };

                session.Store(user);
                session.SaveChanges();

                return user;
            }
        }

        protected Browser BrowseModule<T>() where T : NancyModule
        {
            FormsAuthenticationConfiguration formsConfig;

            return new Browser(with =>
            {
                with.Module<T>();
                with.Dependency<Func<IDocumentSession>>(Store.OpenSession);
                with.ApplicationStartup((ioc, pipelines) => FormsAuthentication.Enable(pipelines, FormsConfig));
            });
        }
    }
}