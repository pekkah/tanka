namespace Tanka.Web.Api
{
    using global::Nancy;
    using global::Nancy.ModelBinding;
    using global::Nancy.Security;
    using Helpers;
    using Infrastructure;
    using Markdown;
    using Markdown.Html;
    using Models;

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
                    return global::System.Net.HttpStatusCode.BadRequest;
                }

                string slug = Snail.ToSlug(slugDto.Text);

                return new {Slug = slug};
            };

            Post["/markdown/render"] = parameters =>
            {
                string content = Request.Body.ReadAsString();

                if (string.IsNullOrEmpty(content))
                    return global::System.Net.HttpStatusCode.BadRequest;

                var parser = new MarkdownParser();
                var htmlRenderer = new HtmlRenderer();

                Document document = parser.Parse(content);
                string html = htmlRenderer.Render(document);

                return Response.AsText(html, "text/html");
            };
        }
    }
}