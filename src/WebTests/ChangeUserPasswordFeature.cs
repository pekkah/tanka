namespace Tanka.WebTests
{
    using FluentAssertions;
    using global::Nancy;
    using global::Nancy.Testing;
    using Web.Api;
    using Web.Documents;
    using Web.Infrastructure;
    using Xunit;

    public class ChangeUserPasswordFeature : FeatureTestBase
    {
        [Fact]
        public void ChangePasswordToValidPassword()
        {
            /* arrange */
            User currentUser = NewUser("tester", SystemRoles.Administrators);
            Browser browser = BrowseModule<UserModule>();

            /* act */
            BrowserResponse response = browser.Post("/api/users/current/password", with =>
            {
                with.HttpsRequest();
                with.FormsAuth(currentUser.Identifier, FormsConfig);
                with.JsonBody(new {password = "fdsfd123543gfdg#ABF"});
            });

            /* assert */
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);
        }
    }
}