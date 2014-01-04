namespace Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CommentDto
    {
        [Required]
        [MinLength(4)]
        public string Content { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Author { get; set; }

        public DateTimeOffset Created { get; set; }

        public int Id { get; set; }

        [Required]
        public int BlogPostId { get; set; }
    }
}