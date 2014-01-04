namespace Web.Infrastructure
{
    using Autofac;

    public interface IRegistration
    {
        void Register(ContainerBuilder builder);
    }
}