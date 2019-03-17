using System.Collections.Generic;

namespace BookStore.Domain.Models
{
    public interface IBookStoreDb
    {
        IRepository<Book> Books { get; }

        IRepository<Publisher> Publishers { get; }

        IEnumerable<Book> GetPublisherBooks(Publisher publisher);
    }
}
