namespace Tanka.Web.Controllers
{
    using System;
    using Infrastructure;
    using Microsoft.AspNet.Authorization;
    using Models;
    using Raven.Client;
    using Microsoft.AspNet.Mvc;
    using Microsoft.AspNet.Mvc.Filters;

    [RequireHttps]
    [Route("installer")]
    [Authorize]
    public class InstallerController : Controller
    {
        private readonly Func<IDocumentSession> _sessionFactory;

        public InstallerController(IDocumentStore sessionFactory)
        {
            _sessionFactory = sessionFactory.OpenSession;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            using (var session = _sessionFactory())
            {
                var settings = session.GetSiteSettings();

                if (!settings.IsInstallerEnabled)
                {
                    context.Result = RedirectToAction("Home", "Admin");
                    return;
                }
            }

            var key = Config.GetValue("Security:InstallerKey");

            if (string.IsNullOrWhiteSpace(key) || key == "null")
            {
                context.Result = RedirectToAction("ConfigNoInstallerKey", "Errors");
                return;
            }

            base.OnActionExecuting(context);
        }

        [Route("install")]
        public ActionResult Install()
        {
            return View();
        }

        [Route("install")]
        [HttpPost]
        public ActionResult Install(AdminDetailsModel model)
        {
            if (!ModelState.IsValid)
                return View();

            var key = Config.GetValue("Security:InstallerKey");

            if (key != model.Key)
            {
                return View();
            }

            using (var session = _sessionFactory())
            {
                var settings = session.GetSiteSettings();
                settings.IsInstallerEnabled = false;
                session.StoreSiteSettings(settings);

                session.SaveChanges();
            }

            return RedirectToAction("Home", "Admin");
        }
    }
}