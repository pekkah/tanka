namespace Tanka.Web.Models
{
    using System.Collections.Generic;

    public class HomeModel
    {
        public IEnumerable<BlogPostDto> Posts { get; set; }

        public int TotalResults { get; set; }
    }
}