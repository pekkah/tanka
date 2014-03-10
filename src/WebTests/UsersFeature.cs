namespace Tanka.WebTests
{
    using System.Collections.Generic;
    using FluentAssertions;
    using global::Nancy;
    using Raven.Database.Linq;
    using Web.Api;
    using Web.Helpers;
    using Web.Models;
    using Xunit;

    public class UsersFeature : FeatureTestBase
    {
        protected override IEnumerable<INancyModule> Modules()
        {
            yield return new UsersModule(Store.OpenSession);
        }

        [Fact]
        public void GetUsers()
        {
            /* given */
            // admin user

            /* when */
            var response = Get("api/users");

            /* then */
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);
            var users = response.ToObject<UsersDto>();

            users.Users.Should().HaveCount(1);
            users.Users.Should().OnlyContain(user => user.Id == Id.WithoutCollection(Admin.Id));
        }

        [Fact]
        public void DeleteUser()
        {
            /* given */
            var createResponse = Post("api/users", new CreateUserRequest()
            {
                UserName = "tester",
                Password = "mypassword"
            });

            /* when */
            var deleteResponse = Delete(createResponse.Headers["Location"]);
            deleteResponse.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);
            
            /* then */
            var users = Get("api/users").ToObject<UsersDto>();
            users.Users.Should().HaveCount(1);
        }
    }
}