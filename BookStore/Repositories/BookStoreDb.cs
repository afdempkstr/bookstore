using BookStore.Domain.Models;
using BookStore.Migrations;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace BookStore.Repositories
{
    public class BookStoreDb : IBookStoreDb, IDisposable
    {
        private DbConnection _connection;

        #region IBookStoreDb interface implementation

        public IRepository<Book> Books { get; }
        public IPublisherRepository Publishers { get; }

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