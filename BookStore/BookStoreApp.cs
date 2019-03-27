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

        #region Publisher Methods

        public OperationResult<IEnumerable<Publisher>> GetPublishers()
        {
            try
            {
                var publishers = _db.Publishers.All();
                return new OperationResult<IEnumerable<Publisher>>(publishers);
            }
            catch (DbException e)
            {
                return new OperationResult<IEnumerable<Publisher>>(e);
            }
        }

        public OperationResult<Publisher> GetPublisher(int id)
        {
            try
            {
                var publisher = _db.Publishers.Find(id);
                return new OperationResult<Publisher>(publisher);
            }
            catch (DbException e)
            {
                return new OperationResult<Publisher>(e);
            }
        }

        public OperationResult<Publisher> Create(Publisher publisher)
        {
            try
            {
                publisher = _db.Publishers.Create(publisher);
                return new OperationResult<Publisher>(publisher);
            }
            catch (DbException e)
            {
                return new OperationResult<Publisher>(e);
            }
        }

        public OperationResult<bool> Update(Publisher publisher)
        {
            throw new System.NotImplementedException();
        }

        public OperationResult Delete(Publisher publisher)
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

        public OperationResult<IEnumerable<Book>> GetBooks(Publisher publisher)
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

        #endregion

        #region Book Methods

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

        #endregion

        public void Dispose()
        {
            _db?.Dispose();
        }
    }
}
