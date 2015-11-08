namespace Tanka.Web.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Documents.Indexes;
    using Raven.Client;

    public class PublishedBlogPostTags
    {
        private readonly IDocumentStore _store;

        public PublishedBlogPostTags(IDocumentStore store)
        {
            _store = store;
        }

        public async Task<IEnumerable<KeyValuePair<string, int>>> GetTagCounts()
        {
            using (var session = _store.OpenAsyncSession())
            {
                var tagCounts = await session.Query<BlogPosts_Published_Tags.Result, BlogPosts_Published_Tags>()
                    .ToListAsync()
                    .ConfigureAwait(false);

                return tagCounts.Select(tag => new KeyValuePair<string, int>(tag.Tag, tag.Count));
            }
        } 
    }
}