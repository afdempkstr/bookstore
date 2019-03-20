using BookStore.Domain.Models;
using System.Collections.Generic;
using System.Data.Common;

namespace BookStore.Application
{
    /// <summary>
    /// Contains all the business logic for the bookstore application.
    /// </summary>
    public class BookStoreApp : IBookStoreApp
    {
        private IBookStoreDb _db;

        public BookStoreApp(IBookStoreDb db)
        {
            _db = db;
        }

        public OperationResult<IEnumerable<Book>> GetBooks()
        {
            try
            {
                var books = _db.Books.All();
                return new OperationResult<IEnumerable<Book>>(books);
            }
            catch (DbException e)
            {
                return new OperationResult<IEnumerable<Book>>(e);
            }
        }

        public OperationResult<IEnumerable<Book>> GetPublisherBooks(Publisher publisher)
        {
            if (publisher == null || publisher.Id <= 0)
            {
                return new OperationResult<IEnumerable<Book>>(false, "Invalid publisher");
            }

            try
            {
                var books = _db.GetPublisherBooks(publisher);
                return new OperationResult<IEnumerable<Book>>(books);
            }
            catch (DbException e)
            {
                return new OperationResult<IEnumerable<Book>>(e);
            }
        }

        public OperationResult DeletePublisher(Publisher publisher)
        {
            if (publisher == null || publisher.Id <= 0)
            {
                return new OperationResult(false, "Invalid publisher");
            }

            try
            {
                var success = _db.Publishers.Delete(publisher.Id);
                return new OperationResult(success);
            }
            catch (DbException e)
            {
                return new OperationResult(e);
            }
        }
    }
}
