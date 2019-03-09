using System.Linq;
using System.Web.Mvc;
using BookStore.Domain.Models;
using BookStore.Migrations;
using BookStore.Repositories;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var bookCount = 0;
            Book book = null;
            using (var db = new BookStoreDb())
            {
                var books = db.Books.All();
                book = books.FirstOrDefault();
                bookCount = books.Count();
            }

            //bookCount = BookStoreApp.GetTotalBookCount();

            ViewBag.TotalBookCount = bookCount;
            ViewBag.Book = $"{book?.Title} by {book.Publisher?.Name}";

            return View();
        }
        
    }
}
