namespace Web.Infrastructure
{
    using System;
    using System.Linq;
    using Documents;
    using Nancy;
    using Nancy.Authentication.Forms;
    using Nancy.Security;
    using Raven.Client;

    public class FormsAuthenticationUserMapper : IUserMapper
    {
        private readonly Func<IDocumentSession> _sessionFactory;

        public FormsAuthenticationUserMapper(Func<IDocumentSession> sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context)
        {
            using (var session = _sessionFactory())
            {
                User user = session.Query<User>().SingleOrDefault(u => u.Identifier == identifier);

                if (user == null)
                    return null;

                return new UserIdentity(user.UserName, user.Roles.ToArray());
            }
        }
    }
}