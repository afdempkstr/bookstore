using System.Collections.Generic;
using System.Data.Common;
using System.Security.Permissions;
using BookStore.Domain.Models;
using Dapper;

namespace BookStore.Repositories
{
    public class BookRepository : RepositoryBase<Book>
    {
        public BookRepository(DbConnection connection, IBookStoreDb parent) 
            : base(connection, parent)
        {
        }

        public override IEnumerable<Book> All()
        {
            return AllWith<Publisher>((book, publisher) => book.Publisher = publisher);
        }

        public override Book Create(Book item)
        {
            //create the publisher first if it does not exist
            if (!(item.Publisher?.Id > 0))
            {
                item.Publisher = Parent.Publishers.Create(item.Publisher);
            }

            var query = $"INSERT INTO {TableName} (Title, Author, CoverPhoto, PublicationYear, PublisherId) " +
                        "VALUES(@Title, @Author, @CoverPhoto, @PublicationYear, @PublisherId); " +
                        "SELECT CAST(SCOPE_IDENTITY() AS int)";
            var insertedItemId = Connection.ExecuteScalar<int>(query, new {
                item.Title,
                item.Author,
                item.CoverPhoto,
                item.PublicationYear,
                PublisherId = item.Publisher.Id
            });
            item.Id = insertedItemId;
            return item;
        }

        public override bool Update(Book item)
        {
            try
            {
                var affectedRows = Connection.Execute(
                    $"UPDATE {TableName} SET Title=@Title, Author=@Author, " +
                        "CoverPhoto=@CoverPhoto, PublicationYear=@PublicationYear, " +
                        "PublisherId=@PublisherId WHERE Id=@Id",
                    new {
                        item.Id,
                        item.Title,
                        item.Author,
                        item.CoverPhoto,
                        item.PublicationYear,
                        PublisherId = item.Publisher.Id
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