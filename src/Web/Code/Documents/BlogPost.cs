namespace Documents
{
    using System;
    using System.Collections.Generic;

    public class Comment
    {
        public string Id { get; set; }

        public string Author { get; set; }

        public string AuthorId { get; set; }

        public string Content { get; set; }

        public DateTimeOffset Created { get; set; }
    }

    public class BlogPost
    {
        public BlogPost()
        {
            CommentIds = new List<string>();
        }

        public string Content { get; set; }

        public string Id { get; set; }

        public string Slug { get; set; }

        public string Title { get; set; }

        public DocumentState State { get; set; }

        public DateTimeOffset? PublishedOn { get; set; }

        public string Author { get; set; }

        public DateTimeOffset Created { get; set; }

        public List<string> CommentIds { get; set; }

        public DateTimeOffset ModifiedOn { get; set; }

        public ICollection<string> Tags { get; set; }
    }

    public enum DocumentState
    {
        Draft,
        Published
    }
}