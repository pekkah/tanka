using System.Web.Configuration;
using Autofac;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Autofac;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Web.Infrastructure.Configuration;
using Web.Infrastructure.RavenDb;
using FormsAuthenticationConfiguration = Nancy.Authentication.Forms.FormsAuthenticationConfiguration;

public class NancyBootstrapper : AutofacNancyBootstrapper
{
    protected override void ApplicationStartup(ILifetimeScope container, IPipelines pipelines)
    {
        Config.GetValueFunc = key => WebConfigurationManager.AppSettings.Get(key);

        JsonConvert.DefaultSettings = (() =>
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
            return settings;
        });
    }

    //protected override void ConfigureApplicationContainer(ILifetimeScope existingContainer)
    //{
    //    // Perform registration that should have an application lifetime
    //}

    //protected override void ConfigureRequestContainer(ILifetimeScope container, NancyContext context)
    //{
    //    // Perform registrations that should have a request lifetime
    //}

    protected override void RequestStartup(ILifetimeScope container, IPipelines pipelines, NancyContext context)
    {
        base.RequestStartup(container, pipelines, context);

        FormsAuthentication.Enable(
            pipelines,
            new FormsAuthenticationConfiguration()
            {
                RedirectUrl = "~/admin/login",
                UserMapper = container.Resolve<IUserMapper>()
            }
            );
    }

    protected override ILifetimeScope GetApplicationContainer()
    {
        var builder = new ContainerBuilder();

        builder.RegisterAssemblyModules(typeof (RavenModule).Assembly);

        return builder.Build();
    }
}