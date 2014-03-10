namespace Tanka.Web.Api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using BCrypt.Net;
    using Documents;
    using global::Nancy;
    using global::Nancy.ModelBinding;
    using global::Nancy.Security;
    using Helpers;
    using Infrastructure;
    using Markdown.Blocks;
    using Models;
    using Raven.Client;
    using HttpStatusCode = global::Nancy.HttpStatusCode;

    public class UsersModule : NancyModule
    {
        public UsersModule(Func<IDocumentSession> sessionFactory)
            : base("api/users")
        {
            this.RequiresHttpsOrXProto();
            this.RequiresAuthentication();

            Post["current/password"] = r =>
            {
                var request = this.BindAndValidate<ChangePasswordRequest>();

                if (!ModelValidationResult.IsValid)
                    return HttpStatusCode.BadRequest;

                string password = request.Password;
                string userName = Context.CurrentUser.UserName;

                using (IDocumentSession session = sessionFactory())
                {
                    User user = session.Query<User>().Single(u => u.UserName == userName);
                    user.Password = BCrypt.HashPassword(password);
                    session.SaveChanges();

                    return HttpStatusCode.OK;
                }
            };

            Get["/"] = r =>
            {
                using (var session = sessionFactory())
                {
                    var users = session.Query<User>().ToList();

                    return new UsersDto
                    {
                        Users = users.Select(user => new UserDto()
                        {
                            Id = Id.WithoutCollection(user.Id),
                            UserName = user.UserName
                        })
                    };
                }
            };

            Post["/"] = r =>
            {
                var request = this.BindAndValidate<CreateUserRequest>();

                if (!ModelValidationResult.IsValid)
                    return HttpStatusCode.BadRequest;

                string password = request.Password;
                string userName = request.UserName;

                using (IDocumentSession session = sessionFactory())
                {
                    if (session.Query<User>().Any(u => u.UserName == userName))
                        return HttpStatusCode.BadRequest;

                    var user = new User
                    {
                        UserName = userName,
                        Password = BCrypt.HashPassword(password),
                        Roles = new[] { SystemRoles.Administrators }
                    };

                    session.Store(user);
                    session.SaveChanges();

                    return Negotiate
                        .WithStatusCode(HttpStatusCode.Created)
                        .WithHeader("Location", "api/users/" + Id.WithoutCollection(user.Id))
                        .WithModel(user);
                }
            };

            Delete["{id}"] = r =>
            {
                if (!r.id.HasValue)
                    return HttpStatusCode.BadRequest;

                using (var session = sessionFactory())
                {
                    var id = (int) r.Id;
                    var user = session.Load<User>(id);

                    if (user == null)
                        return HttpStatusCode.NotFound;

                    if (user.UserName == Context.CurrentUser.UserName)
                        return HttpStatusCode.Forbidden;

                    session.Delete(user);
                    session.SaveChanges();

                    return HttpStatusCode.OK;
                }
            };
        }
    }
}