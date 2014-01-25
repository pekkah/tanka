namespace Tanka.Web.Infrastructure
{
    using System;
    using Documents;
    using global::Nancy;
    using Raven.Client;

    public static class InstallerExtensions
    {
        public static void RequiresInstallerDisabled(this INancyModule module, Func<IDocumentSession> sessionFactory)
        {
            module.Before.AddItemToStartOfPipeline(context =>
            {
                using (IDocumentSession session = sessionFactory())
                {
                    SiteSettings settings = session.GetSiteSettings();

                    if (!settings.IsInstallerEnabled)
                    {
                        return null;
                    }
                }

                string key = Config.GetValue("tanka/installer/key");

                // if installer key present redirect to installer
                if (IsInstallerKeySet(key))
                {
                    return module.Response.AsRedirect("/installer");
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