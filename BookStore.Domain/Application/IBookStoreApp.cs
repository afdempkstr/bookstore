using System;
using BookStore.Domain.Models;
using System.Collections.Generic;

namespace BookStore.Application
{
    public interface IBookStoreApp : IDisposable
    {
        #region Book Methods

        OperationResult<IEnumerable<Book>> GetBooks();

        #endregion

        #region Publisher Methods

        OperationResult<IEnumerable<Publisher>> GetPublishers();

        OperationResult<Publisher> GetPublisher(int id);

        OperationResult<Publisher> Create(Publisher publisher);

        OperationResult<bool> Update(Publisher publisher);

        OperationResult Delete(Publisher publisher);

        OperationResult<IEnumerable<Book>> GetBooks(Publisher publisher);

        #endregion

        // Add other application-level functionality surface methods here, e.g. reporting
    }
}
