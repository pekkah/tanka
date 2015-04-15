namespace Tanka.Web.Documents
{
    using System;
    using System.Collections.Generic;

    public class BlogPost
    {
        public string Content { get; set; }

        public string Id { get; set; }

        public string Slug { get; set; }

        public string Title { get; set; }

        public DocumentState State { get; set; }

        public DateTimeOffset? PublishedOn { get; set; }

        public string Author { get; set; }

        public DateTimeOffset Created { get; set; }

        public DateTimeOffset ModifiedOn { get; set; }

        public ICollection<string> Tags { get; set; }
    }

    public enum DocumentState
    {
        Draft,
        Published
    }
}