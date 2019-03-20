namespace BookStore.Domain.Models
{
    /// <summary>
    /// The base class for all entities stored in the database.
    /// We assume each entity record has a numeric primary key named Id.
    /// </summary>
    public class Entity
    {
        public int Id { get; set; }
    }
}
