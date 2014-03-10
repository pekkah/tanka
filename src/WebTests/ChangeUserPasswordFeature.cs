namespace Tanka.WebTests
{
    using System.Collections.Generic;
    using FluentAssertions;
    using global::Nancy;
    using Web.Api;
    using Xunit;

    public class ChangeUserPasswordFeature : FeatureTestBase
    {
        protected override IEnumerable<INancyModule> Modules()
        {
            yield return new UsersModule(Store.OpenSession);
        }

        [Fact]
        public void ChangePasswordToValidPassword()
        {
            /* given */
            /* when */
            var response = Post("api/users/current/password", new {password = "fdsfd123543gfdg#ABF"});

            /* then */
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);
        }
    }
}