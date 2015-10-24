namespace Tanka.Web.Controllers
{
    using System.Threading.Tasks;
    using Core;
    using Infrastructure;
    using Microsoft.AspNet.Mvc;
    using Models;
    using Raven.Client;

    [Route("history")]
    public class HistoryController : Controller
    {
        private readonly PublishedBlogPosts _publishedBlogPosts;

        public HistoryController(IDocumentStore documentStore, IMarkdownRenderer markdownRenderer)
        {
            _publishedBlogPosts = new PublishedBlogPosts(documentStore, markdownRenderer);
        }

        [Route("{year:int}/{month:int}")]
        public async Task<ActionResult> OfMonth(int year, int month)
        {
            var posts = await _publishedBlogPosts.OfMonth(year, month).ConfigureAwait(false);
            ViewBag.SubTitle = $"{year}/{month}";
            ViewBag.Title = ViewBag.Title;

            return View(new HomeModel() { Posts = posts.Posts, TotalResults = posts.TotalResults });
        }

    }
}