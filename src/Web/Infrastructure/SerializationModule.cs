namespace Web.Infrastructure
{
    using Autofac;
    using Newtonsoft.Json;

    public class SerializationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context =>
                             {
                                 var settings = new JsonSerializerSettings();
                                 settings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());

                                 return JsonSerializer.Create(settings);
                             }).As<JsonSerializer>().SingleInstance();
        }
    }
}