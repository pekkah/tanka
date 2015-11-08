namespace Tanka.Web.Documents.Indexes
{
    using System.Linq;
    using Raven.Abstractions.Indexing;
    using Raven.Client.Indexes;

    public class BlogPosts_Published_Tags : AbstractIndexCreationTask<BlogPost, BlogPosts_Published_Tags.Result>
    {
        public BlogPosts_Published_Tags()
        {
            Map = posts => from post in posts
                where post.PublishedOn.HasValue
                from tag in post.Tags
                select new
                {
                    Tag = tag,
                    Count = 1
                };

            Reduce = results => from tagCount in results
                group tagCount by tagCount.Tag
                into tagGroup
                select new
                {
                    Tag = tagGroup.Key,
                    Count = tagGroup.Sum(t => t.Count)
                };

            Sort(r => r.Count, SortOptions.Int);
        }

        public class Result
        {
            public string Tag { get; set; }

            public int Count { get; set; }
        }
    }
}