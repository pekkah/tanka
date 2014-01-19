namespace Tanka.Web.Site
{
    using System;
    using System.Collections.Generic;
    using Documents;
    using global::Nancy;
    using Infrastructure;
    using Models;
    using Raven.Client;

    public class TagsModule : NancyModule
    {
        public TagsModule(Func<IDocumentSession> sessionFactory) : base("/tags")
        {
            Get["/{tag}"] = parameters =>
            {
                int skip = 0;
                int take = 100;

                if (Request.Query.skip.HasValue)
                {
                    skip = (int) Request.Query.skip;
                }

                if (Request.Query.take.HasValue)
                {
                    take = (int) Request.Query.take;
                }

                if (!parameters.tag.HasValue)
                {
                    return HttpStatusCode.BadRequest;
                }

                var tag = (string) parameters.tag;

                using (IDocumentSession session = sessionFactory())
                {
                    int total;
                    IEnumerable<BlogPostDto> posts = session.GetPublishedBlogPosts(tag, skip, take, out total);
                    SiteSettings site = session.GetSiteSettings();

                    return View["tag", new HomeModel {Posts = posts, SubTitle = tag, Title = site.Title}];
                }
            };
        }
    }
}