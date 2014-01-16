namespace Web
{
    using Autofac;
    using Infrastructure;
    using Nancy;
    using Nancy.Authentication.Forms;
    using Nancy.Bootstrapper;
    using Nancy.Bootstrappers.Autofac;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Tanka.Nancy.Optimization;

    public class NancyBootstrapper : AutofacNancyBootstrapper
    {
        public NancyBootstrapper()
        {
            StaticConfiguration.DisableErrorTraces = false;
        }

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
                    UserMapper = container.Resolve<IUserMapper>()
                }
                );

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