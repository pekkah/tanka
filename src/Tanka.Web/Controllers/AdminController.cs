namespace Tanka.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Documents;
    using Infrastructure;
    using Microsoft.AspNet.Authentication.OpenIdConnect;
    using Microsoft.AspNet.Authorization;
    using Microsoft.AspNet.Http.Authentication;
    using Microsoft.AspNet.Mvc;
    using Raven.Client;

    [Route("admin")]
    [RequireHttps]
    public class AdminController : Controller
    {
        private readonly Func<IDocumentSession> _sessionFactory;

        public AdminController(IDocumentStore sessionFactory)
        {
            _sessionFactory = sessionFactory.OpenSession;
            //this.RequiresInstallerDisabled(sessionFactory);
            //this.RequiresHttpsOrXProto();
            //this.RequiresAuthentication();
        }

        [Route("home")]
        [Authorize()]
        public ActionResult Home()
        {
            using (IDocumentSession session = _sessionFactory())
            {
                SiteSettings site = session.GetSiteSettings();

                if (site == null)
                {
                    site = new SiteSettings
                    {
                        SubTitle = "Go to site -> settings"
                    };
                }

                return View(site);
            }
        }

        [Route("login")]
        public async Task Login()
        {
            await
                ActionContext.HttpContext.Authentication.ChallengeAsync(
                    OpenIdConnectAuthenticationDefaults.AuthenticationScheme);
        }
    }
}