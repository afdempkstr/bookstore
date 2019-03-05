using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        
    }
}
