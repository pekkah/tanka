﻿namespace Tanka.Web.Controllers
{
    using System;
    using Documents;
    using Infrastructure;
    using Microsoft.AspNet.Authorization;
    using Microsoft.AspNet.Mvc;
    using Raven.Client;

    [RequireHttps]
    [Authorize()]
    [Route("_admin")]
    public class AdminController : Controller
    {
        private readonly Func<IDocumentSession> _sessionFactory;

        public AdminController(IDocumentStore sessionFactory)
        {
            _sessionFactory = sessionFactory.OpenSession;
        }

        [Route("index")]
        public ActionResult Home()
        {
            using (IDocumentSession session = _sessionFactory())
            {
                SiteSettings site = session.GetSiteSettings();

                if (site == null)
                {
                    site = new SiteSettings
                    {
                        Title = "Tanka",
                        SubTitle = "Go to site -> settings"
                    };
                }

                ViewBag.Title = site.Title;
                ViewBag.SubTitle = site.SubTitle;
                return View();
            }
        }
    }
}