using BookStore.Domain.Models;

namespace BookStore.Domain.Application
{
    /// <summary>
    /// Defines the operations supported by a bookstore app client
    /// (in our case, the web front-end)
    /// </summary>
    public interface IBookStoreAppClient
    {
        void OnNotify(string message);

        void OnBookAdded(Book book);
    }
}
