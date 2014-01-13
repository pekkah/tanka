namespace Web.Infrastructure
{
    using Autofac;

    public class AuthenticationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LoginService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<FormsAuthenticationUserMapper>().AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}