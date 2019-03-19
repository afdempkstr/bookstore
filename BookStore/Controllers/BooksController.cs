using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Domain.Application;
using BookStore.Domain.Models;
using BookStore.Repositories;

namespace BookStore.Controllers
{
    public class BooksController : Controller
    {
        // GET: Books
        public ActionResult Index()
        {
            OperationResult<IEnumerable<Book>> result = null;

            using (var db = new BookStoreDb())
            {
                var app = new BookStoreApp(db);
                result = app.GetBooks();
            }

            if (!result.Success)
            {
                // display the error message if you wish
                return View("Error", null);
            }

            return View(result.Result);
        }

        // GET: Books/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "Title,Author,PublicationYear")]Book book, int publisherId, HttpPostedFileBase CoverPhoto)
        {
            try
            {
                if (CoverPhoto != null)
                {
                    string path = Path.Combine(Server.MapPath("~/Content/Photos"),
                        Path.GetFileName(CoverPhoto.FileName));
                    CoverPhoto.SaveAs(path);

                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Books/Edit/5
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

        // GET: Books/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Books/Delete/5
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
