namespace Web.Api
{
    using System.ComponentModel.DataAnnotations;
    using Helpers;
    using Nancy;
    using Nancy.ModelBinding;
    using HttpStatusCode = System.Net.HttpStatusCode;

    public class SlugDto
    {
        [Required]
        [MinLength(3)]
        public string Text { get; set; }
    }

    public class UtilsAdminApi : NancyModule
    {
        public UtilsAdminApi()
            : base("/api/admin/utils")
        {
            Post["/slugs"] = parameters =>
            {
                var slugDto = this.Bind<SlugDto>();

                if (string.IsNullOrWhiteSpace(slugDto.Text))
                {
                    return HttpStatusCode.BadRequest;
                }

                string slug = SlugMaker.ToSlug(slugDto.Text);

                return new {Slug = slug};
            };
        }
    }
}