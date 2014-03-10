namespace Tanka.WebTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::Nancy;
    using global::Nancy.Authentication.Forms;
    using global::Nancy.Serialization.JsonNet;
    using global::Nancy.Testing;
    using Nancy.Optimization;
    using Web.Documents;
    using Web.Infrastructure;
    using Web.Installer;

    public abstract class FeatureTestBase : DatabaseTestBase
    {
        private readonly Browser _browser;

        private readonly Dictionary<string, object> _configuration;

        protected FeatureTestBase()
        {
            Bundler.Enable(false);
            BundleTable.Bundles.Clear();

            _configuration = new Dictionary<string, object>();
            Configure(_configuration);

            Config.GetValueFunc = key =>
            {
                object value = null;
                if (!_configuration.TryGetValue(key, out value))
                {
                    throw new InvalidOperationException(
                        string.Format("Test is missing configuration value for key {0}", key));
                }

                return value;
            };

            FormsConfig = new FormsAuthenticationConfiguration
            {
                RedirectUrl = "~/admin/login",
                UserMapper = new FormsAuthenticationUserMapper(Store.OpenSession)
            };

            Serializer = new JsonNetSerializer();
            Deserializer = new JsonNetBodyDeserializer();

            _browser = CreateBrowser();
            Admin = Install("admin");
        }

        protected User Admin { get; set; }

        private JsonNetBodyDeserializer Deserializer { get; set; }

        private JsonNetSerializer Serializer { get; set; }

        private static FormsAuthenticationConfiguration FormsConfig { get; set; }
        protected string AdminPassword { get; set; }

        protected virtual void Configure(IDictionary<string, object> config)
        {
            config.Add("tanka/installer/key", "123-123-abc");
            config.Add("tanka/theme", "/Content/themes/default/bootstrap.css");
            config.Add("tanka/hljs-theme", "/Content/highlight/xcode.css");
            config.Add("disqus/shortname", "tanka-blog");
        }

        protected BrowserResponse Post(string url, object body)
        {
            return _browser.Post(url, with =>
            {
                with.HttpsRequest();
                with.Accept("application/json");

                if (Admin != null)
                    with.FormsAuth(Admin.Identifier, FormsConfig);

                with.JsonBody(body, Serializer);
            });
        }

        protected BrowserResponse Delete(string url)
        {
            return _browser.Delete(url, with =>
            {
                with.HttpsRequest();
                with.Accept("application/json");

                if (Admin != null)
                    with.FormsAuth(Admin.Identifier, FormsConfig);
            });
        }

        protected BrowserResponse Post(string url, Dictionary<string, string> form, string accept = "application/json")
        {
            return _browser.Post(url, with =>
            {
                with.HttpsRequest();
                with.Accept(accept);

                if (Admin != null)
                    with.FormsAuth(Admin.Identifier, FormsConfig);

                foreach (var keyValue in form)
                    with.FormValue(keyValue.Key, keyValue.Value);
            });
        }

        protected BrowserResponse Get(string url, string accept = "application/json")
        {
            return _browser.Get(url, with =>
            {
                with.HttpsRequest();
                with.Accept(accept);
                with.FormsAuth(Admin.Identifier, FormsConfig);
            });
        }

        protected User Install(string adminUserName)
        {
            AdminPassword = Guid.NewGuid().ToString();

            BrowserResponse response = Post("/installer",
                new Dictionary<string, string>
                {
                    {"key", Config.GetValue("tanka/installer/key")},
                    {"username", adminUserName},
                    {"password", AdminPassword}
                });

            response.ShouldHaveRedirectedTo("/admin");

            using (var session = Store.OpenSession())
            {
                return session.Query<User>().Single(u => u.UserName == adminUserName);
            }
        }

        private Browser CreateBrowser()
        {
            return new Browser(with =>
            {
                with.Module(new InstallerModule(Store.OpenSession));

                foreach (var modules in Modules())
                {
                    with.Module(modules);
                }

                with.ApplicationStartup((ioc, pipelines) => { FormsAuthentication.Enable(pipelines, FormsConfig); });
            });
        }

        protected abstract IEnumerable<INancyModule> Modules();
    }
}