namespace Models
{
    using System.Collections.Generic;
    using Documents;

    public class HomeModel
    {
        public IEnumerable<BlogPostDto> Posts { get; set; }
        public int TotalResults { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
    }
}