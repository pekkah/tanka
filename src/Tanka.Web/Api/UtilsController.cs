namespace Tanka.Web.Api
{
    using System;
    using Helpers;
    using Infrastructure;
    using Microsoft.AspNet.Mvc;
    using Microsoft.AspNet.Authorization;

    [Route("api/utils")]

    [Authorize]
    public class UtilsController : Controller
    {
        private readonly IMarkdownRenderer _markdownRenderer;

        public UtilsController(IMarkdownRenderer markdownRenderer)
        {
            _markdownRenderer = markdownRenderer;
        }

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

            string html;

            try
            {
                html = _markdownRenderer.Render(markdown.Markdown);
            }
            catch (Exception x)
            {
                html = "Error in markdown format";
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