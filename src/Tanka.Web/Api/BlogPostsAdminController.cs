namespace Tanka.Web.Api
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Net;
    using System.Security.Claims;
    using Documents;
    using Helpers;
    using Infrastructure;
    using Models;
    using Raven.Client;
    using Raven.Client.Linq;
    using Microsoft.AspNet.Authorization;
    using Microsoft.AspNet.Mvc;

    [Route("api/blog-posts")]
    [Authorize]
    public class BlogPostsAdminController : Controller
    {
        private readonly Func<IDocumentSession> _sessionFactory;

        public BlogPostsAdminController(IDocumentStore documentStore)
        {
            _sessionFactory = documentStore.OpenSession;
        }

        [HttpPost]
        public CreatedResult Post([FromBody]BlogPostDto blogPostDto)
        {
            var statusCode = HttpStatusCode.Created;
            using (var session = _sessionFactory())
            {
                BlogPost blogPost = null;

                if (blogPostDto.Id != null)
                {
                    blogPost = session.Load<BlogPost>(blogPostDto.Id);
                }

                if (blogPost == null)
                {
                    blogPost = new BlogPost
                    {
                        Created = DateTimeOffset.UtcNow
                    };

                    session.Store(blogPost);
                }

                if (blogPostDto.State == DocumentState.Published &&
                    !string.IsNullOrWhiteSpace(blogPostDto.Slug))
                {
                    blogPost.PublishedOn = blogPostDto.PublishedOn ?? DateTimeOffset.UtcNow;
                    blogPost.State = DocumentState.Published;
                }
                else if (blogPostDto.State == DocumentState.Draft)
                {
                    blogPost.PublishedOn = null;
                    blogPost.State = DocumentState.Draft;
                }

                blogPost.Content = blogPostDto.Content;
                blogPost.Author = @User.FindFirst(ClaimTypes.GivenName).Value;
                blogPost.Slug = blogPostDto.Slug;
                blogPost.Title = blogPostDto.Title;
                blogPost.ModifiedOn = DateTime.UtcNow;
                blogPost.Tags = blogPostDto.Tags;

                session.SaveChanges();

                var blogPostId = Id.WithoutCollection(blogPost.Id);

                var location = $"blogposts/{blogPostId}";

                Response.StatusCode = (int)statusCode;
                return Created(location, null);
            }
        }

        [HttpGet]
        public IEnumerable<BlogPostDto> GetAll()
        {
            using (var session = _sessionFactory())
            {
                var skip = 0;
                var take = 100;

                var posts =
                    session.Query<BlogPost>()
                        .Skip(skip)
                        .Take(take)
                        .OrderBy(post => post.State)
                        .ThenByDescending(post => post.PublishedOn)
                        .ThenByDescending(post => post.Created)
                        .ToList();

                return 
                    posts.Select(
                        post => new BlogPostDto
                        {
                            Id = Id.WithoutCollection(post.Id),
                            Content = post.Content,
                            Author = post.Author,
                            Created = post.Created,
                            ModifiedOn = post.ModifiedOn,
                            PublishedOn = post.PublishedOn,
                            Title = post.Title,
                            State = post.State,
                            Slug = post.Slug,
                            Tags = post.Tags ?? new Collection<string>()
                        });
            }
        }

        [HttpGet("{id:int}")]
        public ActionResult Get(int id)
        {
            using (var session = _sessionFactory())
            {
                var blogPost = session.Load<BlogPost>(id);

                if (blogPost == null)
                {
                    return new HttpNotFoundResult();
                }

                return new ObjectResult(new BlogPostDto
                    {
                        Id = Id.WithoutCollection(blogPost.Id),
                        Content = blogPost.Content,
                        Author = blogPost.Author,
                        Created = blogPost.Created,
                        PublishedOn = blogPost.PublishedOn,
                        Title = blogPost.Title,
                        State = blogPost.State,
                        Slug = blogPost.Slug,
                        Tags = blogPost.Tags ?? new Collection<string>(),
                        ModifiedOn = blogPost.ModifiedOn
                    });
            }
        }

        [HttpGet("{id:int}/as-html")]
        public ObjectResult GetAsHtml(int id)
        {
            using (var session = _sessionFactory())
            {
                return new ObjectResult(session.GetRenderedBlogPost(id));
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            try
            {
                using (var session = _sessionFactory())
                {
                    var blogPost = session.Load<BlogPost>(id);

                    session.Delete(blogPost);
                    session.SaveChanges();

                    return new EmptyResult();
                }
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }
    }
}