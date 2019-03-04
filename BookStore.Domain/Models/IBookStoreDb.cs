using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Models
{
    public interface IBookStoreDb
    {
        IRepository<Book> Books { get; }

        IRepository<Publisher> Publishers { get; }
    }
}
