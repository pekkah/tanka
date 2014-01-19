namespace Tanka.Web.Infrastructure
{
    using System;
    using System.Linq;
    using BCrypt.Net;
    using Documents;
    using Raven.Client;

    public class LoginService : ILoginService
    {
        private readonly IDocumentStore _documentStore;

        public LoginService(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public bool Login(string userName, string password, out User user)
        {
            if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException("userName");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException("password");


            using (IDocumentSession session = _documentStore.OpenSession())
            {
                user = session.Query<User>().SingleOrDefault(u => u.UserName == userName);

                if (user == null || !PasswordMatches(user.Password, password))
                {
                    return false;
                }

                return true;
            }
        }

        private bool PasswordMatches(string original, string givenPassword)
        {
            return BCrypt.Verify(givenPassword, original);
        }
    }
}