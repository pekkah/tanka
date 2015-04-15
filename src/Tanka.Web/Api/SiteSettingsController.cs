namespace Tanka.Web.Api
{
    using System;
    using Documents;
    using Infrastructure;
    using Microsoft.AspNet.Authorization;
    using Microsoft.AspNet.Mvc;
    using Raven.Client;

    [Route("api/settings")]
    [Authorize]
    public class SiteSettingsController : Controller
    {
        private readonly Func<IDocumentSession> _sessionFactory;

        public SiteSettingsController(IDocumentStore documentStore)
        {
            _sessionFactory = documentStore.OpenSession;
        }

        [HttpGet]
        public ActionResult Get()
        {
            using (IDocumentSession session = _sessionFactory())
            {
                SiteSettings settings = session.GetSiteSettings();

                if (settings == null)
                {
                    settings = new SiteSettings();
                }

                return new ObjectResult(settings);
            }
        }

        [HttpPut]
        public ActionResult Put([FromBody]SiteSettings settings)
        {
            using (IDocumentSession session = _sessionFactory())
            {
                session.StoreSiteSettings(settings);
                session.SaveChanges();

                return new EmptyResult();
            }
        }
    }
}