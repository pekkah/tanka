namespace Web.Helpers
{
    using System.Reflection;

    public static class Tanka
    {
        public static readonly string Version;

        static Tanka()
        {
            Version = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
        }
    }
}