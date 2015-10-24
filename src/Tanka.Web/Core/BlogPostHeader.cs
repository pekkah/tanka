namespace Tanka.Web.Core
{
    using System;

    public class BlogPostHeader
    {
        public string Id
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string Author { get; set; }

        public DateTime PublishedOn { get; set; }
    }
}