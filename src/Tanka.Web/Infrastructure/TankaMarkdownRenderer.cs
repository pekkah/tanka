namespace Tanka.Web.Infrastructure
{
    public class TankaMarkdownRenderer : IMarkdownRenderer
    {
        public string Render(string markdown)
        {
            return CommonMark.CommonMarkConverter.Convert(markdown);
        }
    }
}