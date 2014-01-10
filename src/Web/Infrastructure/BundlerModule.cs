namespace Web.Infrastructure
{
    using Autofac;
    using Bundles;
    using Tanka.Nancy.Optimization;
    using Tanka.Nancy.Optimization.AjaxMin;

    public class BundlerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ScriptBundler>().AsImplementedInterfaces();
            builder.RegisterType<StyleBundler>().AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(typeof (ThemeStyleBundle).Assembly)
                .Where(type => typeof (StyleBundle).IsAssignableFrom(type))
                .As<StyleBundle>()
                .SingleInstance();

            builder.RegisterAssemblyTypes(typeof (CoreScriptBundle).Assembly)
                .Where(type => typeof (ScriptBundle).IsAssignableFrom(type))
                .As<ScriptBundle>()
                .SingleInstance();
        }
    }
}