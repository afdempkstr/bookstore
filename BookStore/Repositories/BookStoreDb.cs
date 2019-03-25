using BookStore.Domain.Models;
using BookStore.Migrations;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FluentMigrator.Runner.Initialization;

namespace BookStore.Repositories
{
    public class BookStoreDb : IBookStoreDb, IDisposable
    {
        private readonly DbConnection _connection;

        #region IBookStoreDb interface implementation

        public IRepository<Book> Books { get; }

        public IRepository<Publisher> Publishers { get; }

        public IRepository<User> Users { get; }

        public IEnumerable<Book> GetPublisherBooks(Publisher publisher)
        {
            IEnumerable<Book> books = Enumerable.Empty<Book>();

            if (publisher != null)
            {
                books = Books.All().Where(book => book.Publisher.Id == publisher.Id);
            }

            return books;
        }

        public bool CheckUserCredentials(string username, string password)
        {
            var userId = _connection.ExecuteScalar<int>("dbo.CheckUserCredentials",
                new
                {
                    Username = username,
                    Password = password
                },
                commandType: CommandType.StoredProcedure);

            return userId > 0;
        }

        public void SetUserCredentials(string username, string password)
        {
            _connection.ExecuteScalar<int>("dbo.SetUserCredentials",
                new
                {
                    Username = username,
                    Password = password
                },
                commandType: CommandType.StoredProcedure);
        }

        #endregion

        private static string ConnectionString => Properties.Settings.Default.ConnectionString;

        private static DbConnection ConnectionFactory()
        {
            return new SqlConnection(ConnectionString);
        }

        static BookStoreDb()
        {
            if (!UpdateDatabase())
            {
                throw new ApplicationException("The database migrations were unsuccessful");
            }
        }

        public BookStoreDb()
        {
            _connection = ConnectionFactory();
            _connection.Open();
            Books = new BookRepository(_connection, this);
            Publishers = new PublisherRepository(_connection, this);
            Users = new UserRepository(_connection, this);
        }

        #region Db Migrations

        private static bool UpdateDatabase()
        {
            try
            {
                var serviceProvider = ConfigureDbMigrations();

                using (var scope = serviceProvider.CreateScope())
                {
                    RunDbMigrations(scope.ServiceProvider);
                }

                return true;
            }
            catch (DbException)
            {
                return false;
            }
        }

        private static IServiceProvider ConfigureDbMigrations()
        {
            return new ServiceCollection()
                // Add common FluentMigrator services
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    // Add SQLite support to FluentMigrator
                    .AddSqlServer()
                    // Set the connection string
                    .WithGlobalConnectionString(ConnectionString)
                    // Define the assembly containing the migrations
                    .ScanIn(typeof(AddBookTable).Assembly).For.Migrations())
                // Enable logging to console in the FluentMigrator way
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                // Build the service provider
                .BuildServiceProvider(false);
        }

        private static void RunDbMigrations(IServiceProvider serviceProvider)
        {
            // Instantiate the runner
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            // Execute the migrations
            runner.MigrateUp();
        }

        #endregion

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}