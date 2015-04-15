namespace Tanka.Web.Controllers
{
    using System;
    using BCrypt.Net;
    using Documents;
    using Infrastructure;
    using Models;
    using Raven.Client;
    using Microsoft.AspNet.Mvc;

    [RequireHttps]
    [Route("_installer")]
    public class InstallerController : Controller
    {
        private readonly Func<IDocumentSession> _sessionFactory;

        public InstallerController(IDocumentStore sessionFactory)
        {
            _sessionFactory = sessionFactory.OpenSession;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            using (var session = _sessionFactory())
            {
                var settings = session.GetSiteSettings();

                if (!settings.IsInstallerEnabled)
                {
                    filterContext.Result = RedirectToAction("Index", "Admin");
                    return;
                }
            }

            var key = Config.GetValue("tanka/installer/key");

            if (string.IsNullOrWhiteSpace(key) || key == "null")
            {
                filterContext.Result = RedirectToAction("ConfigNoInstallerKey", "Errors");
                return;
            }

            base.OnActionExecuting(filterContext);
        }

        [Route("")]
        public ActionResult Install()
        {
            return View();
        }

        [Route("")]
        [HttpPost]
        public ActionResult Install(AdminDetailsModel model)
        {
            if (!ModelState.IsValid)
                return View();

            var key = Config.GetValue("tanka/installer/key");

            if (key != model.Key)
            {
                return View();
            }

            using (var session = _sessionFactory())
            {
                var user = new User
                {
                    UserName = model.Username,
                    Identifier = Guid.NewGuid(),
                    Password = BCrypt.HashPassword(model.Password),
                    Roles = new[]
                    {
                        SystemRoles.Administrators
                    }
                };

                session.Store(user);

                var settings = session.GetSiteSettings();
                settings.IsInstallerEnabled = false;
                session.StoreSiteSettings(settings);

                session.SaveChanges();
            }

            return RedirectToAction("Index", "Admin");
        }
    }
}