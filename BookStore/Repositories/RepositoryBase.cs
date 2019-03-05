using BookStore.Domain.Models;
using Dapper;
using System.Collections.Generic;
using System.Data.Common;

namespace BookStore.Repositories
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : Entity
    {
        protected DbConnection Connection { get; }

        protected string TableName { get; }

        public RepositoryBase(DbConnection connection)
        {
            Connection = connection;
            TableName = typeof(T).Name;
        }

        public IEnumerable<T> All()
        {
            return Connection.Query<T>($"SELECT * FROM {TableName}");
        }

        public virtual bool Delete(int id)
        {
            try
            {
                var affectedRows = Connection.Execute($"DELETE FROM {TableName} WHERE Id=@Id", 
                    new {Id = id});

                return affectedRows > 0;
            }
            catch (DbException)
            {
                return false;
            }
        }

        public virtual T Find(int id)
        {
            return Connection.QueryFirstOrDefault<T>($"SELECT * FROM {TableName} WHERE Id=@Id",
                new {Id = id});
        }

        public abstract T Create(T item);

        public abstract bool Update(T item);
    }
}