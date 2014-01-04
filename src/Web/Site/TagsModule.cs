namespace Web.Site
{
    using Infrastructure;
    using Models;
    using Nancy;
    using Raven.Client;

    public class TagsModule : NancyModule
    {
        public TagsModule(IDocumentSession documentSession) : base("/tags")
        {
            Get["/{tag}"] = parameters =>
            {
                int skip = 0;
                int take = 100;

                if (Request.Query.skip.HasValue)
                {
                    skip = (int)Request.Query.skip;
                }

                if (Request.Query.take.HasValue)
                {
                    take = (int) Request.Query.take;
                }

                if (!parameters.tag.HasValue)
                {
                    return HttpStatusCode.BadRequest;
                }

                var tag = (string)parameters.tag;
                int total;
                var posts = documentSession.GetPublishedBlogPosts(tag, skip, take, out total);
                var site = documentSession.GetSiteSettings();

                return View["tag", new HomeModel() {Posts = posts, SubTitle = tag, Title = site.Title}];
            };
        }
    }
}