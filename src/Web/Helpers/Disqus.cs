namespace Tanka.Web.Helpers
{
    using System.Text;
    using global::Nancy.ViewEngines.Razor;
    using Infrastructure;

    public static class Disqus
    {
        public static IHtmlString Render(string identifier)
        {
            string shortname = Config.GetValue("disqus/shortname");
            var builder = new StringBuilder();

            if (string.IsNullOrWhiteSpace(identifier))
            {
                builder.AppendLine("<script type=\"text/javascript\">");
                builder.AppendLine(string.Format("var disqus_shortname = '{0}'; ", shortname));
                builder.AppendLine("</script>");
                builder.AppendLine(
                    string.Format("<script async type=\"text/javascript\" src=\"//{0}.disqus.com/count.js\"></script>",
                        shortname));
            }
            else
            {
                builder.AppendLine("<script type=\"text/javascript\">");
                builder.AppendLine(string.Format("var disqus_shortname = '{0}'; ", shortname));
                builder.AppendLine(string.Format("var disqus_identifier = '{0}';", identifier));
                builder.AppendLine("</script>");
                builder.AppendLine(
                    string.Format("<script async type=\"text/javascript\" src=\"//{0}.disqus.com/embed.js\"></script>",
                        shortname));
                builder.AppendLine(
                    string.Format("<script async type=\"text/javascript\" src=\"//{0}.disqus.com/count.js\"></script>",
                        shortname));
            }

            string script = builder.ToString();
            return new NonEncodedHtmlString(script);
        }
    }
}