namespace Tanka.Web.Api
{
    using Helpers;
    using Markdown;
    using Markdown.Html;
    using Microsoft.AspNet.Mvc;
    using Microsoft.AspNet.Authorization;

    [Route("api/utils")]

    [Authorize]
    public class UtilsAdminController : Controller
    {
        [HttpPost("slugs")]
        public ActionResult Slugify([FromBody]ToSlug toSlug)
        {
            if (string.IsNullOrWhiteSpace(toSlug.Text))
            {
                return new EmptyResult();
            }

            string slug = Snail.ToSlug(toSlug.Text);
            
            return new ObjectResult(slug);
        }

        [HttpPost("markdown/render")]
        public ActionResult AsHtml([FromBody]ToHtml markdown)
        {
            if (string.IsNullOrEmpty(markdown.Markdown))
                return new BadRequestResult();

            var parser = new MarkdownParser();
            var htmlRenderer = new MarkdownHtmlRenderer();

            string html = string.Empty;

            try
            {
                Document document = parser.Parse(markdown.Markdown);
                html = htmlRenderer.Render(document);
            }
            catch (ParsingException x)
            {
                html = $"Markdown parsing error at {x.Position} as block type {x.BuilderType}";
            }
            catch (RenderingException renderingException)
            {
                html =
                    $"Markdown rendering error with block {renderingException.Block} using {renderingException.Renderer} renderer";
            }

            return new ObjectResult(html);
        }
    }

    public class ToSlug
    {
        public string Text { get; set; }
    }

    public class ToHtml
    {
        public string Markdown { get; set; }
    }
}