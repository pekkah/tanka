namespace Tanka.WebTests
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using global::Nancy;
    using global::Nancy.Testing;
    using Web.Api;
    using Web.Documents;
    using Web.Helpers;
    using Web.Models;
    using Web.Site;
    using Xunit;

    public class BlogFeature : FeatureTestBase
    {
        protected override IEnumerable<INancyModule> Modules()
        {
            yield return new BlogPostsAdminApi(Store.OpenSession);
            yield return new BlogModule(Store.OpenSession);
        }

        private void PublishBlogPost(BlogPostDto post)
        {
            post.State = DocumentState.Published;
            post.PublishedOn = DateTimeOffset.Now;

            var result = Post("api/admin/blogposts", post);
            result.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);
        }

        private BlogPostDto WriteBlogPost(string title, string content)
        {
            var post = new BlogPostDto
            {
                Title = title,
                Slug = Snail.ToSlug(title),
                Content = content,
                ModifiedOn = DateTimeOffset.UtcNow,
                PublishedOn = null
            };

            var result = Post("api/admin/blogposts", post);

            result.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.Created);

            var getResult = Get(result.Headers["Location"]);

            return getResult.ToObject<BlogPostDto>();
        }

        [Fact]
        public void ShotPostsOnHome()
        {
            /* given */
            var blogPost1 = WriteBlogPost("Blog Post 1", "Content of the blog post");
            PublishBlogPost(blogPost1);

            /* when */
            BrowserResponse response = Get("/", "text/html");

            /* then */
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);

            // should contain the blogPost1
            response.Body[".article"].ShouldExistOnce();

            // should contain a link pointing to full blog post
            var linkToBlogPost1 = string.Format("a[href=\"/{0}\"]", blogPost1.Slug);
            response.Body[linkToBlogPost1]
                .ShouldExistOnce()
                .And.ShouldContain(blogPost1.Title);
        }
    }
}