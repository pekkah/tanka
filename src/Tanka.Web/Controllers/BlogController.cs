namespace Tanka.Web.Controllers
{
    using System;
    using Infrastructure;
    using Microsoft.AspNet.Authorization;
    using Models;
    using Raven.Client;
    using Microsoft.AspNet.Mvc;

    [Route("")]
    public class BlogController : Controller
    {
        private readonly Func<IDocumentSession> _sessionFactory;

        public BlogController(IDocumentStore sessionFactory)
        {
            _sessionFactory = sessionFactory.OpenSession;
            //this.RequiresInstallerDisabled(sessionFactory);
        }

        [Route("", Order=1000)]
        public ActionResult Home(int skip = 0, int take = 10)
        {
            var model = new HomeModel();

            using (IDocumentSession session = _sessionFactory())
            {
                var total = 0;

                var posts = session.GetPublishedBlogPosts(skip, take, out total);
                var site = session.GetSiteSettings();

                // todo: refactor into attribute
                if (site == null)
                    return RedirectToAction("Offline", "Errors");

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
            using (IDocumentSession session = _sessionFactory())
            {
                var post = session.GetPublishedBlogPost(slug);

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