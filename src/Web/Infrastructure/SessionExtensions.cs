namespace Tanka.Web.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Documents;
    using Helpers;
    using Markdown;
    using Markdown.Html;
    using Models;
    using Raven.Client;
    using Raven.Client.Linq;

    public static class SessionExtensions
    {
        public static Configuration GetConfiguration(this IDocumentSession session)
        {
            var configuration = session.Load<Configuration>("Tanka/System/Configuration");

            return configuration;
        }

        public static void StoreConfiguration(this IDocumentSession session, Configuration configuration)
        {
            session.Store(configuration, "Tanka/System/Configuration");
        }

        public static SiteSettings GetSiteSettings(this IDocumentSession session)
        {
            var configuration = session.Load<SiteSettings>("Tanka/Settings/Site");

            return configuration;
        }

        public static void StoreSiteSettings(this IDocumentSession session, SiteSettings settings)
        {
            session.Store(settings, "Tanka/Settings/Site");
        }

        public static IEnumerable<BlogPostDto> GetPublishedBlogPosts(this IDocumentSession session, int skip,
            int pageSize, out int total)
        {
            RavenQueryStatistics stats;
            List<BlogPostDto> posts = session.Query<BlogPost>()
                .Statistics(out stats)
                .Where(post => post.State == DocumentState.Published)
                .Where(post => post.PublishedOn <= DateTimeOffset.UtcNow)
                .OrderByDescending(post => post.PublishedOn)
                .Skip(skip)
                .Take(pageSize)
                .ToList()
                .Select(ToDto).ToList();

            total = stats.TotalResults;

            return posts;
        }

        public static IEnumerable<BlogPostDto> GetPublishedBlogPosts(this IDocumentSession session, string tag, int skip,
            int pageSize, out int total)
        {
            RavenQueryStatistics stats;
            List<BlogPostDto> posts = session.Query<BlogPost>()
                .Statistics(out stats)
                .Where(post => post.State == DocumentState.Published)
                .Where(post => post.PublishedOn <= DateTimeOffset.UtcNow)
                .Where(post => post.Tags.Any(t => t == tag))
                .OrderByDescending(post => post.PublishedOn)
                .Skip(skip)
                .Take(pageSize)
                .ToList()
                .Select(ToDto).ToList();

            total = stats.TotalResults;

            return posts;
        }

        public static BlogPostDto GetPublishedBlogPost(this IDocumentSession session, string slug)
        {
            BlogPost blogPost =
                session.Query<BlogPost>()
                    .SingleOrDefault(post => post.State == DocumentState.Published && post.Slug == slug);

            if (blogPost == null)
            {
                return null;
            }

            return ToDto(blogPost);
        }

        private static BlogPostDto ToDto(BlogPost blogPost)
        {
            string html = string.Empty;

            if (!string.IsNullOrWhiteSpace(blogPost.Content))
            {
                var md = new MarkdownParser();
                var renderer = new HtmlRenderer();
                Document document = md.Parse(blogPost.Content);

                html = renderer.Render(document);
            }

            return new BlogPostDto
            {
                Content = html,
                Author = blogPost.Author,
                Created = blogPost.Created,
                PublishedOn = blogPost.PublishedOn,
                Title = blogPost.Title,
                Slug = blogPost.Slug,
                Id = Id.WithoutCollection(blogPost.Id),
                Tags = blogPost.Tags ?? new Collection<string>()
            };
        }

        public static BlogPostDto GetRenderedBlogPost(this IDocumentSession session, int id)
        {
            var blogPost = session.Load<BlogPost>(id);

            if (blogPost == null)
            {
                return null;
            }

            return ToDto(blogPost);
        }
    }
}