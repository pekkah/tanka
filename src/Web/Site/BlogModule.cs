namespace Web.Site
{
    using Infrastructure.RavenDb;
    using Models;
    using Nancy;
    using Raven.Client;

    public class BlogModule : NancyModule
    {
        public BlogModule(IDocumentStore documentStore)
        {
            Get["/"] = parameters =>
                       {
                           var model = new HomeModel();

                           using (var session = documentStore.OpenSession())
                           {
                               var total = 0;
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

                               var posts = session.GetPublishedBlogPosts(skip, take, out total);
                               var site = session.GetSiteSettings();

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

                                 using (var session = documentStore.OpenSession())
                                 {
                                     var post = session.GetPublishedBlogPost(slug);

                                     if (post == null)
                                         return 404;

                                     var site = session.GetSiteSettings();
                                     return View["blogpost", new {Post = post, Title = site.Title, SubTitle = post.Title}];
                                 }
                             };
        }
    }
}