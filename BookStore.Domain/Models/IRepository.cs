using System.Collections.Generic;

namespace BookStore.Domain.Models
{
    public interface IRepository<T> where T: Entity
    {
        IEnumerable<T> All();

        T Create(T item);

        T Find(int id);

        bool Update(T item);

        bool Delete(int id);
    }
}
