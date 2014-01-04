namespace Web.Infrastructure.Configuration
{
    using System.Configuration;
    using Autofac;

    public class ConfigurationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(GetConfiguration).As<WebConfiguration>().SingleInstance();
        }

        public static WebConfiguration GetConfiguration(IComponentContext context)
        {
            return new WebConfiguration
                {
                    ReadValue = (key) => ConfigurationManager.AppSettings.Get(key)
                };
        }
    }
}