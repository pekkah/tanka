namespace Web.Api
{
    using Helpers;
    using Infrastructure;
    using Nancy;
    using Nancy.ModelBinding;
    using Nancy.Security;
    using Tanka.Markdown;
    using Tanka.Markdown.Html;
    using Tanka.Web.Infrastructure;
    using Tanka.Web.Models;
    using HttpStatusCode = System.Net.HttpStatusCode;

    public class UtilsAdminApi : NancyModule
    {
        public UtilsAdminApi()
            : base("/api/admin/utils")
        {
            this.RequiresHttpsOrXProto();
            this.RequiresAuthentication();
            this.RequiresClaims(new[] {SystemRoles.Administrators});

            Post["/slugs"] = parameters =>
            {
                var slugDto = this.Bind<SlugDto>();

                if (string.IsNullOrWhiteSpace(slugDto.Text))
                {
                    return HttpStatusCode.BadRequest;
                }

                string slug = Snail.ToSlug(slugDto.Text);

                return new {Slug = slug};
            };

            Post["/markdown/render"] = parameters =>
            {
                string content = Request.Body.ReadAsString();

                if (string.IsNullOrEmpty(content))
                    return HttpStatusCode.BadRequest;

                var parser = new MarkdownParser();
                var htmlRenderer = new HtmlRenderer();

                Document document = parser.Parse(content);
                string html = htmlRenderer.Render(document);

                return Response.AsText(html, "text/html");
            };
        }
    }
}