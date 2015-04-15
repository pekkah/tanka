namespace Tanka.Web.Api
{
    using System.Linq;
    using Documents;
    using Helpers;
    using Models;
    using Raven.Client;
    using Microsoft.AspNet.Mvc;
    using System;
    using Microsoft.AspNet.Authorization;

    [Route("api/users")]
    [Authorize]
    public class UsersController : Controller
    {
        public const int MaxUsersBeforePaging = 100;

        private readonly Func<IDocumentSession> _sessionFactory;

        public UsersController(IDocumentStore documentStore)
        {
            _sessionFactory = documentStore.OpenSession;
        }

        [HttpGet]
        public UsersDto GetAll()
        {
            using (var session = _sessionFactory())
            {
                var users = session.Query<User>().Take(MaxUsersBeforePaging);

                return new UsersDto()
                {
                    Users = users.Select(user => new UserDto
                    {
                        Id = Id.WithoutCollection(user.Id),
                        UserName = user.UserName
                    })
                };
            }
        }

        [HttpPost]
        public ActionResult Post([FromBody]CreateUserRequest createUserRequest)
        {
            using (var session = _sessionFactory())
            {
                var existingUser = session.Query<User>()
                    .Customize(c => c.WaitForNonStaleResultsAsOfNow())
                    .Any(u => u.UserName == createUserRequest.UserName);

                if (existingUser)
                    return new BadRequestResult();

                var user = new User()
                {
                    UserName = createUserRequest.UserName,
                    Password = BCrypt.Net.BCrypt.HashPassword(createUserRequest.Password),
                    Roles = createUserRequest.Roles
                };

                session.Store(user);
                session.SaveChanges();

                return Get(Id.WithoutCollection(user.Id));
            }
        }

        [HttpGet("{id:int}")]
        private ActionResult Get(int id)
        {
            using (var session = _sessionFactory())
            {
                var user = session.Load<User>(id);

                if (user == null)
                    return null;

                return new ObjectResult(new UserDto
                {
                    Id = Id.WithoutCollection(user.Id),
                    UserName = user.UserName
                });
            }
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            using (var session = _sessionFactory())
            {
                var usersCount = session.Query<User>().Count();

                if (usersCount <= 1)
                    return new BadRequestResult();

                var user = session.Load<User>(id);

                if (user == null)
                    return new HttpNotFoundResult();

                session.Delete(user);
                session.SaveChanges();

                return new EmptyResult();
            }
        }
    }
}