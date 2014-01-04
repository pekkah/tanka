namespace Models
{
    using System.Collections.Generic;

    public class BlogPostsDto
    {
        public IEnumerable<BlogPostDto> Posts { get; set; }

        public int TotalResults { get; set; }
    }
}