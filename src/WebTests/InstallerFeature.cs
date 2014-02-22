namespace Tanka.WebTests
{
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using global::Nancy;
    using Web.Documents;
    using Web.Infrastructure;
    using Xunit;

    public class InstallerFeature : FeatureTestBase
    {
        protected override IEnumerable<INancyModule> Modules()
        {
            /*******************************************************
             * Installer module is added by FeatureTestBase
             * 
             * yield return new InstallerModule(Store.OpenSession);
             * *****************************************************/
            yield break;
        }

        [Fact]
        public void CheckStatus()
        {
            /* arrange */
            /* act */
            // done in test base

            /* assert */
            using (var session = Store.OpenSession())
            {
                // installer is disabled
                var settings = session.GetSiteSettings();
                settings.IsInstallerEnabled.ShouldBeEquivalentTo(false);

                // admin is administrators
                var user = session.Query<User>().Single(u => u.UserName == Admin.UserName);
                user.Roles.Should().Contain(SystemRoles.Administrators);
            }
        }
    }
}