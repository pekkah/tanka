namespace Web.Infrastructure
{
    using Autofac;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public class SerializationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context =>
            {
                var settings = new JsonSerializerSettings();
                settings.Converters.Add(new StringEnumConverter());

                return JsonSerializer.Create(settings);
            }).As<JsonSerializer>().SingleInstance();
        }
    }
}