namespace Tanka.Web.Infrastructure
{
    using Markdown;
    using Markdown.Html;

    public class TankaMarkdownRenderer : IMarkdownRenderer
    {
        public string Render(string markdown)
        {
            var md = new MarkdownParser();
            var renderer = new MarkdownHtmlRenderer();

            Document document = md.Parse(markdown);
            return renderer.Render(document);
        }
    }
}