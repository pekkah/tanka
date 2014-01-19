namespace Tanka.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using Documents;

    public class BlogPostDto
    {
        public int? Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(250)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public DocumentState State { get; set; }

        [Required]
        [MinLength(3)]
        public string Slug { get; set; }

        public DateTimeOffset? PublishedOn { get; set; }

        public DateTimeOffset? Created { get; set; }

        public string Author { get; set; }

        public DateTimeOffset ModifiedOn { get; set; }

        public ICollection<string> Tags { get; set; }
    }
}