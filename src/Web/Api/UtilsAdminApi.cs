namespace Tanka.Web.Api
{
    using System;
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
                var htmlRenderer = new MarkdownHtmlRenderer();

                string html = string.Empty;

                try
                {
                    Document document = parser.Parse(content);
                    html = htmlRenderer.Render(document);
                }
                catch (ParsingException x)
                {
                    html = string.Format(
                        "Markdown parsing error at {0} as block type {1}",
                        x.Position,
                        x.BuilderType);
                }
                catch (RenderingException renderingException)
                {
                    html = string.Format(
                        "Markdown rendering error with block {0} using {1} renderer",
                        renderingException.Block,
                        renderingException.Renderer);
                }

                return Response.AsText(html, "text/html");
               
            };
        }
    }
}