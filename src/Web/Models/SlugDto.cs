namespace Tanka.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    public class SlugDto
    {
        [Required]
        [MinLength(3)]
        public string Text { get; set; }
    }
}