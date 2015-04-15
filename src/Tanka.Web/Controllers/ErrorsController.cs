namespace Tanka.Web.Controllers
{
    using Microsoft.AspNet.Mvc;

    public class ErrorsController : Controller
    {
        public ActionResult Offline()
        {
            return View();
        }
    }
}