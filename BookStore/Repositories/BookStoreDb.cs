using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using BookStore.Domain.Models;
using BookStore.Migrations;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.Repositories
{
    public class BookStoreDb : IBookStoreDb
    {
        #region IBookStoreDb interface implementation

        public IRepository<Book> Books { get; }
        public IRepository<Publisher> Publishers { get; }

        #endregion

        private string ConnectionString => Properties.Settings.Default.ConnectionString;

        private DbConnection ConnectionFactory()
        {
            return new SqlConnection(ConnectionString);
        }

        public BookStoreDb()
        {
            if (!UpdateDatabase())
            {
                throw new ApplicationException("The database migrations were unsuccessful");
            }

            var connection = ConnectionFactory();
            connection.Open();
            Books = new BookRepository(connection);
            Publishers = new PublisherRepository(connection);
        }

        #region Db Migrations

        private bool UpdateDatabase()
        {
            try
            {
                var serviceProvider = ConfugureDbMigrations();

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

        private IServiceProvider ConfugureDbMigrations()
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

        private void RunDbMigrations(IServiceProvider serviceProvider)
        {
            // Instantiate the runner
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            // Execute the migrations
            runner.MigrateUp();
        }

        #endregion
    }
}