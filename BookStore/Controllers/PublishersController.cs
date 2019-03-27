using BookStore.Domain.Models;
using BookStore.Repositories;
using BookStore.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BookStore.Application;

namespace BookStore.Controllers
{
    public class PublishersController : Controller
    {
        private IBookStoreApp _app;

        public PublishersController(IBookStoreApp app)
        {
            _app = app;
        }

        // GET: Publisher
        public ActionResult Index()
        {
            var publishers = Enumerable.Empty<Publisher>();
            using (var db = new BookStoreDb())
            {
                publishers = db.Publishers.All();
            }

            var result = _app.GetPublishers();
            if (!result.Success)
            {
                //notify the user by printing some message
                ViewBag.ErrorMessage = result.ErrorMessage ?? "Could not get publishers";
            }
            else
            {
                publishers = result.Result;
            }

            return View(publishers);
        }

        // GET: Publisher/Details/5
        public ActionResult Details(int id)
        {
            Publisher publisher = null;
            IEnumerable<Book> books = null;

            using (var db = new BookStoreDb())
            {
                publisher = db.Publishers.Find(id);
                books = db.GetPublisherBooks(publisher);
            }

            var model = new PublisherBooks(publisher, books);
            return View(model);
        }

        // GET: Publisher/Create
        [Authorize(Roles = "Employee,Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Publisher/Create
        [Authorize(Roles = "Employee,Admin")]
        [HttpPost]
        public ActionResult Create([Bind(Include = "Name")]Publisher publisher)
        {
            if (string.IsNullOrWhiteSpace(publisher.Name))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                using (var db = new BookStoreDb())
                {
                    var existing = db.Publishers.All().FirstOrDefault(p => p.Name == publisher.Name);
                    if (existing == null)
                    {
                        publisher = db.Publishers.Create(publisher);
                    }
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "Employee,Admin")]
        // GET: Publisher/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Publisher publisher = null;
            using (var db = new BookStoreDb())
            {
                publisher = db.Publishers.Find(id.Value);
            }

            if (publisher == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return View(publisher);
        }

        [Authorize(Roles = "Employee,Admin")]
        // POST: Publisher/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,Name")]Publisher publisher)
        {
            try
            {
                var success = false;
                using (var db = new BookStoreDb())
                {
                    success = db.Publishers.Update(publisher);
                }

                if (success)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("publisher", "The publisher update failed");
                    return View(publisher);
                }
            }
            catch
            {
                return View(publisher);
            }
        }

        // GET: Publisher/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Publisher publisher = null;

            using (var db = new BookStoreDb())
            {
                publisher = db.Publishers.Find(id.Value);
            }

            if (publisher == null)
            {
                return HttpNotFound();
            }

            return View(publisher);
        }

        // POST: Publisher/Delete/5
        [Authorize(Roles = "Employee,Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Publisher publisher = null;

                using (var db = new BookStoreDb())
                {
                    publisher = db.Publishers.Find(id);
                    if (publisher == null)
                    {
                        return HttpNotFound();
                    }
                    else
                    {
                        db.Publishers.Delete(id);
                    }
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
