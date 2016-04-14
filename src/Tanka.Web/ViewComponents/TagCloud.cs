namespace Tanka.Web.ViewComponents
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core;
    using Microsoft.AspNetCore.Mvc;
    using Raven.Client;

    public class TagCloud : ViewComponent
    {
        private readonly PublishedBlogPostTags _tagCloud;

        public TagCloud(IDocumentStore documentStore)
        {
            _tagCloud = new PublishedBlogPostTags(documentStore);
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var tags = await _tagCloud.GetTagCounts().ConfigureAwait(false);
            tags = tags.Select(tag => new KeyValuePair<string, int>(tag.Key, Normalize(tag.Value)))
                .OrderByDescending(t => t.Value);

            return View(tags);
        }

        private int Normalize(int value)
        {
            if (value < 6)
                return value;

            return 6;
        }
    }
}