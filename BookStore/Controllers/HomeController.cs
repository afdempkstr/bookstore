using System.Linq;
using System.Web.Mvc;
using BookStore.Repositories;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var bookCount = 0;
            using (var db = new BookStoreDb())
            {
                bookCount = db.Books.All().Count();
            }

            //bookCount = BookStoreApp.GetTotalBookCount();

            ViewBag.TotalBookCount = bookCount;

            return View();
        }
        
    }
}
