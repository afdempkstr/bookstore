using System.Data.Common;
using BookStore.Domain.Models;
using Dapper;

namespace BookStore.Repositories
{
    public class BookRepository : RepositoryBase<Book>
    {
        public BookRepository(DbConnection connection) : base(connection)
        {
        }

        public override Book Create(Book item)
        {
            var query = $"INSERT INTO {TableName} (Title, Author, CoverPhoto, PublicationYear) " +
                        "VALUES(@Title, @Author, @CoverPhoto, @PublicationYear); SELECT CAST(SCOPE_IDENTITY() AS int)";
            var insertedItemId = Connection.ExecuteScalar<int>(query, new {
                item.Title,
                item.Author,
                item.CoverPhoto,
                item.PublicationYear });
            item.Id = insertedItemId;
            return item;
        }

        public override bool Update(Book item)
        {
            try
            {
                var affectedRows = Connection.Execute(
                    $"UPDATE {TableName} SET Title=@Title, Author=@Author, " +
                        "CoverPhoto=@CoverPhoto, PublicationYear=@PublicationYear " +
                        "WHERE Id=@Id",
                    new {
                        item.Id,
                        item.Title,
                        item.Author,
                        item.CoverPhoto,
                        item.PublicationYear
                    });

                return affectedRows > 0;
            }
            catch (DbException)
            {
                return false;
            }
        }
    }
}