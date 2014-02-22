namespace Tanka.Web.Models
{
    using System;
    using System.Collections.Generic;
    using Documents;
    using FluentValidation;

    public class BlogPostDto
    {
        public int? Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DocumentState State { get; set; }

        public string Slug { get; set; }

        public DateTimeOffset? PublishedOn { get; set; }

        public DateTimeOffset? Created { get; set; }

        public string Author { get; set; }

        public DateTimeOffset? ModifiedOn { get; set; }

        public ICollection<string> Tags { get; set; }
    }

    public class BlogPostValidator : AbstractValidator<BlogPostDto>
    {
        public BlogPostValidator()
        {
            RuleFor(p => p.Title).NotEmpty().Length(3, 200);
            RuleFor(p => p.Slug).NotEmpty().Length(3, 255);
            RuleFor(p => p.Author).NotNull();
        }
    }
}