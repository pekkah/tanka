namespace Tanka.Web.Infrastructure
{
    using global::Nancy;

    public static class InstallerExtensions
    {
        public static void RequiresInstallerDisabled(this INancyModule module, bool redirect = false)
        {
            module.Before.AddItemToStartOfPipeline(context =>
            {
                string key = Config.GetValue("tanka/installer/key");

                // if installer key present redirect to installer
                if (IsInstallerKeySet(key))
                {
                    if (redirect)
                        return module.Response.AsRedirect("/installer");

                    return module.Response.AsText(
                        "<h1>Temporarily offline. Installer enabled. Please set key to 'null'</h1>",
                        "text/html");
                }

                return null;
            });
        }

        public static bool IsInstallerKeySet(string value)
        {
            return !string.IsNullOrWhiteSpace(value) && value != "null";
        }
    }
}