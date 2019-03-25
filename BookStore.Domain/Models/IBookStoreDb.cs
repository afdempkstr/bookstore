using System.Collections.Generic;

namespace BookStore.Domain.Models
{
    public interface IBookStoreDb
    {
        IRepository<Book> Books { get; }

        IRepository<Publisher> Publishers { get; }

        IRepository<User> Users { get; }

        IEnumerable<Book> GetPublisherBooks(Publisher publisher);

        bool CheckUserCredentials(string username, string password);

        void SetUserCredentials(string username, string password);
    }
}
