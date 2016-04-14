namespace Tanka.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class ErrorsController : Controller
    {
        public ActionResult Offline()
        {
            return View();
        }
    }
}