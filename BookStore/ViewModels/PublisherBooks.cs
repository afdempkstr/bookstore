using BookStore.Domain.Models;
using System.Collections.Generic;

namespace BookStore.ViewModels
{
    public class PublisherBooks
    {
        public Publisher Publisher { get; }
        public IEnumerable<Book> Books { get; }

        public PublisherBooks(Publisher publisher, IEnumerable<Book> books)
        {
            Publisher = publisher;
            Books = books;
        }
    }
}