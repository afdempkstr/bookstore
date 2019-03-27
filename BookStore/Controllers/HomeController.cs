using BookStore.Application;
using System.Linq;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        private IBookStoreApp _app;

        public HomeController(IBookStoreApp app)
        {
            _app = app;
        }

        // GET: Home
        public ActionResult Index()
        {
            var bookCount = 0;
            //Book book = null;
            //using (var db = new BookStoreDb())
            //{
            //    var books = db.Books.All();
            //    book = books.FirstOrDefault();
            //    bookCount = books.Count();
            //}

            var result = _app.GetBooks();
            if (!result.Success)
            {
                ViewBag.ErrorMessage = result.ErrorMessage ?? result.Exception?.Message;
                ViewBag.Book = null;
            }
            else
            {
                var books = result.Result;
                var book = books.FirstOrDefault();
                ViewBag.Book = $"{book?.Title} by {book.Publisher?.Name}";
                bookCount = books.Count();
            }

            ViewBag.TotalBookCount = bookCount;
            return View();
        }
        
    }
}
