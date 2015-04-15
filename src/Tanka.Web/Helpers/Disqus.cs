namespace Tanka.Web.Helpers
{
    using System.Text;
    using System.Web;
    using Infrastructure;
    using Microsoft.AspNet.Mvc.Rendering;

    public static class Disqus
    {
        public static HtmlString Render(string identifier)
        {
            string shortname = Config.GetValue("disqus/shortname");
            var builder = new StringBuilder();

            if (string.IsNullOrWhiteSpace(identifier))
            {
                builder.AppendLine("<script type=\"text/javascript\">");
                builder.AppendLine($"var disqus_shortname = '{shortname}'; ");
                builder.AppendLine("</script>");
                builder.AppendLine($"<script type=\"text/javascript\" src=\"//{shortname}.disqus.com/count.js\"></script>");
            }
            else
            {
                builder.AppendLine("<script type=\"text/javascript\">");
                builder.AppendLine($"var disqus_shortname = '{shortname}'; ");
                builder.AppendLine($"var disqus_identifier = '{identifier}';");
                builder.AppendLine("</script>");
                builder.AppendLine($"<script type=\"text/javascript\" src=\"//{shortname}.disqus.com/embed.js\"></script>");
                builder.AppendLine($"<script type=\"text/javascript\" src=\"//{shortname}.disqus.com/count.js\"></script>");
            }

            string script = builder.ToString();
            return new HtmlString(script);
        }
    }
}