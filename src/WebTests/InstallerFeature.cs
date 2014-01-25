namespace Tanka.WebTests
{
    using System.Linq;
    using FluentAssertions;
    using global::Nancy;
    using global::Nancy.Testing;
    using Web.Documents;
    using Web.Infrastructure;
    using Web.Installer;
    using Xunit;

    public class InstallerFeature : FeatureTestBase
    {
        public InstallerFeature()
        {
            Config.GetValueFunc = key =>
            {
                if (key == "tanka/installer/key")
                    return InstallerKey;

                return null;
            };
        }

        public string InstallerKey
        {
            get { return "InstallKey"; }
        }

        [Fact]
        public void ShowView()
        {
            /* arrange */
            Browser browser = BrowseModule<InstallerModule>();

            /* act */
            BrowserResponse response = browser.Get("/installer",
                with => with.HttpsRequest());

            /* assert */
            response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);
        }

        [Fact]
        public void PostForm()
        {
            /* arrange */
            Browser browser = BrowseModule<InstallerModule>();

            /* act */
            BrowserResponse response = browser.Post("/installer",
                with =>
                {
                    with.HttpsRequest();
                    with.FormValue("key", InstallerKey);
                    with.FormValue("username", "tester");
                    with.FormValue("password", "123123");
                });

            /* assert */
            response.ShouldHaveRedirectedTo("/admin");

            using (var session = Store.OpenSession())
            {
                var user = session.Query<User>().Single(u => u.UserName == "tester");

                user.UserName.ShouldBeEquivalentTo("tester");
                user.Password.Should().NotBe("123123");
                user.Roles.Should().Contain(SystemRoles.Administrators);
            }
        }
    }
}