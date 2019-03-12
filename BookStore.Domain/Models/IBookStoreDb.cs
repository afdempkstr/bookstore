namespace BookStore.Domain.Models
{
    public interface IBookStoreDb
    {
        IRepository<Book> Books { get; }

        IPublisherRepository Publishers { get; }
    }
}
