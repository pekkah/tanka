namespace Tanka.Web.ViewComponents
{
    using System.Threading.Tasks;
    using Core;
    using Infrastructure;
    using Microsoft.AspNetCore.Mvc;
    using Raven.Client;

    public class History : ViewComponent
    {
        private readonly IDocumentStore _documentStore;
        private readonly IMarkdownRenderer _markdownRenderer;

        public History(IDocumentStore documentStore, IMarkdownRenderer markdownRenderer)
        {
            _documentStore = documentStore;
            _markdownRenderer = markdownRenderer;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var blogPosts = new PublishedBlogPosts(_documentStore, _markdownRenderer);
            var history = await blogPosts.GetMonthlyHistoryAsync().ConfigureAwait(false);

            return View(history);
        }
    }
}