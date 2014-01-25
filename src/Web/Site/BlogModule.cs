namespace Tanka.Web.Site
{
    using System.Collections.Generic;
    using Documents;
    using global::Nancy;
    using Infrastructure;
    using Models;
    using Raven.Client;

    public class BlogModule : NancyModule
    {
        public BlogModule(IDocumentStore documentStore)
        {
            this.RequiresInstallerDisabled();

            Get["/"] = parameters =>
            {
                var model = new HomeModel();

                using (IDocumentSession session = documentStore.OpenSession())
                {
                    int total = 0;
                    int skip = 0;
                    int take = 10;

                    if (parameters.skip.HasValue)
                    {
                        skip = (int) parameters.skip;
                    }

                    if (parameters.take.HasValue)
                    {
                        take = parameters.take;
                    }

                    IEnumerable<BlogPostDto> posts = session.GetPublishedBlogPosts(skip, take, out total);
                    SiteSettings site = session.GetSiteSettings();

                    if (site == null)
                        return Response.AsText("<h1>Temporarily offline</h1>", "text/html");

                    model.Title = site.Title;
                    model.SubTitle = site.SubTitle;
                    model.Posts = posts;
                    model.TotalResults = total;
                }

                return View[model];
            };

            Get["/{slug}"] = parameters =>
            {
                var slug = (string) parameters.slug;

                using (IDocumentSession session = documentStore.OpenSession())
                {
                    BlogPostDto post = session.GetPublishedBlogPost(slug);

                    if (post == null)
                        return 404;

                    SiteSettings site = session.GetSiteSettings();
                    return View["blogpost", new {Post = post, site.Title, SubTitle = post.Title}];
                }
            };
        }
    }
}