using BookStore.Domain.Models;
using Dapper;
using System.Data.Common;

namespace BookStore.Repositories
{
    public class PublisherRepository : RepositoryBase<Publisher>
    {
        public PublisherRepository(DbConnection connection) 
            : base(connection)
        {
        }

        public override Publisher Create(Publisher item)
        {
            var query = $"INSERT INTO {TableName} (Name) VALUES(@Name); SELECT CAST(SCOPE_IDENTITY() AS int)";
            var insertedItemId = Connection.ExecuteScalar<int>(query, new {Name = item.Name});
            item.Id = insertedItemId;
            return item;
        }

        public override bool Update(Publisher item)
        {
            try
            {
                var affectedRows = Connection.Execute($"UPDATE {TableName} SET Name=@Name WHERE Id=@Id",
                    new {Id = item.Id, Name = item.Name});

                return affectedRows > 0;
            }
            catch (DbException)
            {
                return false;
            }
        }
    }
}