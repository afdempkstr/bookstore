using BookStore.Domain.Models;
using System.Collections.Generic;

namespace BookStore.Domain.Application
{
    public interface IBookStoreApp
    {
        OperationResult<IEnumerable<Book>> GetBooks();

        OperationResult<IEnumerable<Book>> GetPublisherBooks(Publisher publisher);

        OperationResult DeletePublisher(Publisher publisher);
    }
}
