namespace BookStore.Domain.Models
{
    public interface IBookStoreDb
    {
        IRepository<Book> Books { get; }

        IRepository<Publisher> Publishers { get; }
    }
}
