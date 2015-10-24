namespace Tanka.Web.Documents.Indexes
{
    using System.Collections.Generic;
    using System.Linq;
    using Documents;
    using Raven.Client.Indexes;

    public class BlogPosts_Published_Monthly : AbstractIndexCreationTask<BlogPost, BlogPosts_Published_Monthly.Result>
    {
        public class Result
        {
            public int Year { get; set; }

            public int Month { get; set; }

            public int Count { get; set; }

            public IEnumerable<string> Posts { get; set; } 
        }

        public BlogPosts_Published_Monthly()
        {
            Map = posts => from post in posts
                where post.PublishedOn.HasValue
                select new
                {
                    post.PublishedOn.Value.Year,
                    post.PublishedOn.Value.Month,
                    Count = 1,
                    Posts = new List<string>()
                    {
                        post.Id
                    }
                };

            Reduce = results => from result in results
                group result by new { result.Year, result.Month }
                into monthly
                select new
                {
                    monthly.Key.Year,
                    monthly.Key.Month,
                    Count = monthly.Sum(p => p.Count),
                    Posts = monthly.SelectMany(m => m.Posts)
                };
        }
    }
}