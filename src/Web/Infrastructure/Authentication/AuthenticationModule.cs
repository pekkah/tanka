namespace Web.Infrastructure.Authentication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Autofac;
    using Documents;
    using Nancy;
    using Nancy.Authentication.Forms;
    using Nancy.Security;
    using Raven.Client;

    public class AuthenticationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LoginService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<FormsAuthenticationUserMapper>().AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }

    public class FormsAuthenticationUserMapper : IUserMapper
    {
        private readonly IDocumentSession _documentSession;

        public FormsAuthenticationUserMapper(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context)
        {
            User user = _documentSession.Query<User>().SingleOrDefault(u => u.Identifier == identifier);

            return new UserIdentity(user.UserName, user.Roles.ToArray());
        }
    }

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