namespace Tanka.Web.Core
{
    using System;
    using Raven.Client;
    using System.Linq;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Documents;
    using Documents.Indexes;
    using Helpers;
    using Infrastructure;
    using Models;
    using Raven.Client.Linq;

    public class PublishedBlogPosts
    {
        private readonly IDocumentStore _store;
        private readonly IMarkdownRenderer _markdownRenderer;

        public PublishedBlogPosts(IDocumentStore store, IMarkdownRenderer markdownRenderer)
        {
            _store = store;
            _markdownRenderer = markdownRenderer;
        }


        public async Task<IEnumerable<PostsOnMonth>> GetMonthlyHistoryAsync()
        {
            using (var session = _store.OpenAsyncSession())
            {
                var monthlyPosts = await session.Query<BlogPosts_Published_Monthly.Result, BlogPosts_Published_Monthly>()
                    .OrderByDescending(p => p.Year)
                    .OrderByDescending(p => p.Month)
                    .ToListAsync()
                    .ConfigureAwait(false);

                return monthlyPosts.Select(m => new PostsOnMonth()
                {
                    Year = m.Year,
                    Month = m.Month,
                    Count = m.Count
                });
            }
        }

        public async Task<BlogPostsDto> OfMonth(int year, int month, int skip = 0, int take = 100)
        {
            using (var session = _store.OpenAsyncSession())
            {
                var postsOnMonth = await session.Query<BlogPosts_Published_Monthly.Result, BlogPosts_Published_Monthly>()
                    .Where(r => r.Year == year && r.Month == month)
                    .Customize(c => c.Include<BlogPosts_Published_Monthly.Result>(r => r.Posts))
                    .SingleOrDefaultAsync()
                    .ConfigureAwait(false);

                List<BlogPost> monthlyPosts = new List<BlogPost>();

                foreach(var postId in postsOnMonth.Posts)
                {
                    var blogPost = await session.LoadAsync<BlogPost>(postId).ConfigureAwait(false);

                    if (blogPost == null)
                        continue;

                    monthlyPosts.Add(blogPost);
                }

                var result =  new BlogPostsDto()
                {
                    Posts = monthlyPosts.Select(post => ToDto(post, _markdownRenderer)),
                    TotalResults = monthlyPosts.Count
                };

                return result;
            }
        }

        private static BlogPostDto ToDto(BlogPost blogPost, IMarkdownRenderer markdownRenderer)
        {
            string html = string.Empty;

            if (!string.IsNullOrWhiteSpace(blogPost.Content))
            {
                try
                {
                    html = markdownRenderer.Render(blogPost.Content);
                }
                catch (Exception x)
                {
                    html = "Markdown error";
                }
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
    }
}
