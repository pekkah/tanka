namespace Tanka.Web.Controllers
{
    using System;
    using Infrastructure;
    using Models;
    using Raven.Client;
    using Microsoft.AspNetCore.Mvc;

    [Route("tags")]
    public class TagsController : Controller
    {
        private readonly IMarkdownRenderer _markdownRenderer;
        private readonly Func<IDocumentSession> _sessionFactory;

        public TagsController(IDocumentStore sessionFactory, IMarkdownRenderer markdownRenderer)
        {
            _markdownRenderer = markdownRenderer;
            _sessionFactory = sessionFactory.OpenSession;
            //this.RequiresInstallerDisabled(sessionFactory);
        }

        [Route("{tag}")]
        public ActionResult Tag(string tag, int skip = 0, int take = 100)
        {
            using (IDocumentSession session = _sessionFactory())
            {
                int total;
                var posts = session.GetPublishedBlogPosts(tag, skip, take, out total, _markdownRenderer);
                var site = session.GetSiteSettings();
                ViewBag.SubTitle = tag;
                ViewBag.Title = site.Title;

                return View(new HomeModel {Posts = posts, TotalResults = total});
            }
        }
    }
}