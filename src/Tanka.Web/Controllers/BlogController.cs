namespace Tanka.Web.Controllers
{
    using Infrastructure;
    using Models;
    using Raven.Client;
    using Microsoft.AspNet.Mvc;

    [Route("")]
    public class BlogController : Controller
    {
        private readonly IMarkdownRenderer _markdownRenderer;
        private readonly IDocumentStore _documentStore;

        public BlogController(IDocumentStore documentStore, IMarkdownRenderer markdownRenderer)
        {
            _markdownRenderer = markdownRenderer;
            _documentStore = documentStore;
        }

        [Route("", Order=1000)]
        public ActionResult Home(int skip = 0, int take = 100)
        {
            var model = new HomeModel();

            using (IDocumentSession session = _documentStore.OpenSession())
            {
                var site = session.GetSiteSettings();

                // todo: refactor into attribute
                if (site == null)
                    return RedirectToAction("Offline", "Errors");

                var total = 0;
                var posts = session.GetPublishedBlogPosts(skip, take, out total, _markdownRenderer);
                
                ViewBag.Title = site.Title;
                ViewBag.SubTitle = site.SubTitle;

                model.Posts = posts;
                model.TotalResults = total;

            }

            return View(model);
        }

        [Route("{slug}", Order = 1000)]
        public ActionResult BlogPost(string slug)
        {
            using (IDocumentSession session = _documentStore.OpenSession())
            {
                var post = session.GetPublishedBlogPost(slug, _markdownRenderer);

                if (post == null)
                    return HttpNotFound();

                var site = session.GetSiteSettings();
                ViewBag.Title = site.Title;
                ViewBag.SubTitle = post.Title;
                return View(new BlogPostModel
                {
                    Post = post
                });
            }
        }
    }
}