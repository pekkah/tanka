namespace Web.Infrastructure.Authentication.Identities
{
    using System.Security.Principal;

    public class AnonymousUser : IIdentity
    {
        public AnonymousUser()
        {
            IsAuthenticated = false;
        }

        #region IIdentity Members

        public string Name { get; private set; }
        public string AuthenticationType { get; private set; }
        public bool IsAuthenticated { get; private set; }

        #endregion
    }
}