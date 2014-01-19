namespace Tanka.Web
{
    using Autofac;
    using global::Nancy.Authentication.Forms;
    using global::Nancy.Bootstrapper;
    using global::Nancy.Bootstrappers.Autofac;
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