namespace Web.Helpers
{
    using System.Text;
    using Infrastructure;
    using Nancy.ViewEngines.Razor;

    public static class Disqus
    {
        public static IHtmlString Render(string identifier)
        {
            string shortname = Config.GetValue("disqus/shortname");

            var builder = new StringBuilder();
            builder.AppendLine("<script>");
            builder.AppendLine(
                string.Format("var disqus_shortname = '{0}'; ", shortname));
            builder.AppendLine("</script>");
            


            if (!string.IsNullOrWhiteSpace(identifier))
            {
                builder.AppendLine("<script>");
                builder.AppendLine(
                    string.Format("var disqus_identifier = '{0}';", identifier));
                builder.AppendLine("</script>");
                builder.AppendLine(
                string.Format("<script src=\"//{0}.disqus.com/embed.js\" async />", shortname));
            }

            builder.AppendLine(
                    string.Format("<script src=\"//{0}.disqus.com/count.js\" async />", shortname));

            string script = builder.ToString();
            return new NonEncodedHtmlString(script);
        }
    }
}