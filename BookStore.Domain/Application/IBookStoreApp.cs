using BookStore.Domain.Models;
using System.Collections.Generic;

namespace BookStore.Application
{
    public interface IBookStoreApp
    {
        #region Book Methods

        OperationResult<IEnumerable<Book>> GetBooks();

        #endregion

        #region Publisher Methods

        OperationResult<Publisher> GetPublisher(int id);

        OperationResult<Publisher> Create(Publisher publisher);

        OperationResult<bool> Update(Publisher publisher);

        OperationResult Delete(Publisher publisher);

        OperationResult<IEnumerable<Book>> GetBooks(Publisher publisher);

        #endregion


    }
}
