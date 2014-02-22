namespace Tanka.Web.Api
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Documents;
    using global::Nancy;
    using global::Nancy.ModelBinding;
    using global::Nancy.Security;
    using Helpers;
    using Infrastructure;
    using Models;
    using Raven.Client;
    using Raven.Client.Linq;

    public class BlogPostsAdminApi : NancyModule
    {
        public BlogPostsAdminApi(Func<IDocumentSession> sessionFactory)
            : base("/api/admin/blogposts")
        {
            this.RequiresHttpsOrXProto();
            this.RequiresAuthentication();
            this.RequiresClaims(new[] {SystemRoles.Administrators});

            Post["/"] = _ =>
            {
                HttpStatusCode statusCode = HttpStatusCode.OK;

                using (IDocumentSession session = sessionFactory())
                {
                    BlogPost blogPost = null;
                    var blogPostDto = this.BindAndValidate<BlogPostDto>();

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
                        statusCode = HttpStatusCode.Created;
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
                    blogPost.Author = Context.CurrentUser.UserName;
                    blogPost.Slug = blogPostDto.Slug;
                    blogPost.Title = blogPostDto.Title;
                    blogPost.ModifiedOn = DateTime.UtcNow;
                    blogPost.Tags = blogPostDto.Tags;

                    session.SaveChanges();

                    int blogPostId = Id.WithoutCollection(blogPost.Id);

                    string location = string.Format("/api/admin/blogposts/{0}", blogPostId);

                    return Negotiate
                        .WithStatusCode(statusCode)
                        .WithHeader("Location", location)
                        .WithModel(blogPostId);
                }
            };

            Get["/"] = _ =>
            {
                using (IDocumentSession session = sessionFactory())
                {
                    int skip = 0;
                    int take = 100;

                    List<BlogPost> posts =
                        session.Query<BlogPost>()
                            .Skip(skip)
                            .Take(take)
                            .OrderBy(post => post.State)
                            .ThenByDescending(post => post.PublishedOn)
                            .ThenByDescending(post => post.Created)
                            .ToList();

                    return Negotiate
                        .WithStatusCode(HttpStatusCode.OK)
                        .WithModel(
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
                                }));
                }
            };

            Get["/{id}"] = parameters =>
            {
                 if (!parameters.Id.HasValue)
                    return HttpStatusCode.BadRequest;

                using (IDocumentSession session = sessionFactory())
                {
                    var blogPost = session.Load<BlogPost>((int) parameters.Id);

                    if (blogPost == null)
                    {
                        return HttpStatusCode.NotFound;
                    }

                    return Negotiate
                        .WithStatusCode(HttpStatusCode.OK)
                        .WithModel(
                            new BlogPostDto
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
            };

            Get["/{id}/html"] = parameters =>
            {
                if (!parameters.id.HasValue)
                    return HttpStatusCode.BadRequest;

                using (IDocumentSession session = sessionFactory())
                {
                    return session.GetRenderedBlogPost((int) parameters.id);
                }
            };

            Delete["/{id}"] = parameters =>
            {
                try
                {
                    if (!parameters.Id.HasValue)
                        return HttpStatusCode.BadRequest;

                    using (IDocumentSession session = sessionFactory())
                    {
                        var id = (int) parameters.Id;
                        var blogPost = session.Load<BlogPost>(id);

                        session.Delete(blogPost);
                        session.SaveChanges();

                        return HttpStatusCode.OK;
                    }
                }
                catch (Exception)
                {
                    return HttpStatusCode.BadRequest;
                }
            };
        }
    }
}