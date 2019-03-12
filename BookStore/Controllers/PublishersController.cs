using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Domain.Models;
using BookStore.Repositories;
using BookStore.ViewModels;

namespace BookStore.Controllers
{
    public class PublishersController : Controller
    {
        // GET: Publisher
        public ActionResult Index()
        {
            var publishers = Enumerable.Empty<Publisher>();
            using (var db = new BookStoreDb())
            {
                publishers = db.Publishers.All();
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
                books = db.Publishers.GetPublisherBooks(publisher);
            }

            var model = new PublisherBooks(publisher, books);
            return View(model);
        }

        

        // GET: Publisher/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Publisher/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Publisher/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Publisher/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Publisher/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Publisher/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
