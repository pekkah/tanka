namespace Tanka.Web.Api
{
    using System;
    using System.Linq;
    using System.Net;
    using Documents;
    using Raven.Client;
    using Raven.Client.Linq;
    using BCrypt.Net;
    using Microsoft.AspNet.Mvc;
    using Microsoft.AspNet.Authorization;

    [Authorize]
    [Route("api/users/current/password")]
    public class CurrentUserPasswordController : Controller
    {
        private readonly Func<IDocumentSession> _sessionFactory;

        public CurrentUserPasswordController(IDocumentStore documentStore)
        {
            _sessionFactory = documentStore.OpenSession;;
        }

        [HttpPost("")]
        public ActionResult Post([FromBody]string password)
        {
            using (var sesssion = _sessionFactory())
            {
                var userName = User.Identity.Name;

                var user = sesssion.Query<User>().SingleOrDefault(u => u.UserName == userName);

                // if user does not exist but is authorized then something is wrong
                if (user == null)
                    return new HttpNotFoundResult();

                user.Password = BCrypt.HashPassword(password);
                sesssion.SaveChanges();

                return new EmptyResult();
            }
        }
    }
}