namespace Tanka.Web
{
    using System.Web.UI.WebControls;
    using Autofac;
    using global::Nancy;
    using global::Nancy.Authentication.Forms;
    using global::Nancy.Bootstrapper;
    using global::Nancy.Bootstrappers.Autofac;
    using global::Nancy.Responses;
    using Infrastructure;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Tanka.Nancy.Optimization;

    public class NancyBootstrapper : AutofacNancyBootstrapper
    {
        protected override void ApplicationStartup(ILifetimeScope container, IPipelines pipelines)
        {
            JsonConvert.DefaultSettings = (() =>
            {
                var settings = new JsonSerializerSettings();
                settings.Converters.Add(new StringEnumConverter {CamelCaseText = true});
                return settings;
            });

            FormsAuthentication.Enable(
                pipelines,
                new FormsAuthenticationConfiguration
                {
                    RedirectUrl = "~/admin/login",
                    UserMapper = container.Resolve<IUserMapper>(),
                    RequiresSSL = true
                });

            pipelines.BeforeRequest.AddItemToStartOfPipeline(context =>
            {
                if (context.Request.Url.Path == "/installer")
                    return null;

                var key = Config.GetValue("tanka/installer/key");

                // if installer key present redirect to installer
                if (!string.IsNullOrWhiteSpace(key) && key != "null")
                    return new RedirectResponse("/installer");

                return null;
            });

#if DEBUG
            Bundler.Enable(false);
#else
            Bundler.Enable(true);
#endif
        }

        protected override ILifetimeScope GetApplicationContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyModules(typeof (RavenModule).Assembly);

            return builder.Build();
        }
    }
}