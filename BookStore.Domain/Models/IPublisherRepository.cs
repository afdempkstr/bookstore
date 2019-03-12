using System.Collections.Generic;

namespace BookStore.Domain.Models
{
    public interface IPublisherRepository : IRepository<Publisher>
    {
        IEnumerable<Book> GetPublisherBooks(Publisher publisher);
    }
}
