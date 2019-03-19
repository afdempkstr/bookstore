using System.Collections.Generic;

namespace BookStore.Domain.Models
{
    /// <summary>
    /// Defines the basic operations that can be performed on an entity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T: Entity
    {
        /// <summary>
        /// Gets all entities of type T
        /// </summary>
        /// <returns>An enumeration of all items of type T</returns>
        IEnumerable<T> All();

        /// <summary>
        /// Stores an item in the database
        /// </summary>
        /// <param name="item">The item to store</param>
        /// <returns>The item stored. The Id property will be updated with the primary key of the row.</returns>
        T Create(T item);

        /// <summary>
        /// Finds an item by id
        /// </summary>
        /// <param name="id">The item id</param>
        /// <returns>The item with the given id, null if not found</returns>
        T Find(int id);

        /// <summary>
        /// Updates an item in the database
        /// </summary>
        /// <param name="item">The item to update</param>
        /// <returns>True on successful update</returns>
        bool Update(T item);

        /// <summary>
        /// Deletes an item from the database
        /// </summary>
        /// <param name="id">The item id</param>
        /// <returns>True if the item was found and deleted, false otherwise</returns>
        bool Delete(int id);
    }
}
