namespace Tanka.Web.Helpers
{
    using System.Reflection;

    public static class TankaVersion
    {
        public static readonly string Version;

        static TankaVersion()
        {
            Version =
                Assembly.GetExecutingAssembly()
                    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                    .InformationalVersion;
        }
    }
}