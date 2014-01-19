namespace Tanka.Web.Infrastructure
{
    using System.Collections.Generic;
    using global::Nancy.Security;

    public class UserIdentity : IUserIdentity
    {
        public UserIdentity(string userName, params string[] claims)
        {
            UserName = userName;
            Claims = claims;
        }

        public string UserName { get; private set; }
        public IEnumerable<string> Claims { get; private set; }
    }
}