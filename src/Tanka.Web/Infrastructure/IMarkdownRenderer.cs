namespace Tanka.Web.Infrastructure
{
    public interface IMarkdownRenderer
    {
        string Render(string markdown);
    }
}