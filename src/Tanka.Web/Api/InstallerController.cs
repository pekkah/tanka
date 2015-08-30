namespace Tanka.Web.Api
{
    using System;
    using System.Net;
    using BCrypt.Net;
    using Documents;
    using Infrastructure;
    using Models;
    using Raven.Client;
    using Microsoft.AspNet.Mvc;
    using Microsoft.AspNet.Authorization;

    [Route("api/installer")]

    [Authorize]
    public class InstallerController : Controller
    {
        private readonly Func<IDocumentSession> _sessionFactory;

        public InstallerController(IDocumentStore documentStore)
        {
           _sessionFactory = documentStore.OpenSession;
        }

        //protected override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    using (var session = _sessionFactory())
        //    {
        //        var settings = session.GetSiteSettings();

        //        if (!settings.IsInstallerEnabled)
        //        {
        //            filterContext.Result = RedirectToAction("Index", "Admin");
        //            return;
        //        }
        //    }

        //    var key = Config.GetValue("tanka/installer/key");

        //    if (string.IsNullOrWhiteSpace(key) || key == "null")
        //    {
        //        filterContext.Result = RedirectToAction("ConfigNoInstallerKey", "Errors");
        //        return;
        //    }

        //    base.OnActionExecuting(filterContext);
        //}

        [HttpPost("")]
        public ActionResult Post([FromBody]AdminDetailsModel model)
        {
            var key = Config.GetValue("tanka/installer/key");

            if (key != model.Key || string.IsNullOrWhiteSpace(key))
            {
                return new HttpStatusCodeResult((int)HttpStatusCode.InternalServerError);
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

            return RedirectToAction("Home", "Admin");
        }
    }
}