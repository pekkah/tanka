namespace Tanka.Web.Infrastructure
{
    using System;
    using System.Linq;
    using Documents;
    using global::Nancy;
    using global::Nancy.Authentication.Forms;
    using global::Nancy.Security;
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
            using (IDocumentSession session = _sessionFactory())
            {
                User user = session.Query<User>().SingleOrDefault(u => u.Identifier == identifier);

                if (user == null)
                    return null;

                return new UserIdentity(user.UserName, user.Roles.ToArray());
            }
        }
    }
}