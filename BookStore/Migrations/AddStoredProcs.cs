using System;
using FluentMigrator;

namespace BookStore.Migrations
{
    [Migration(20190320030000)]
    public class AddStoredProcs : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
            CREATE IF NOT EXISTS PROCEDURE dbo.CheckUserCredentials
                @Username NVARCHAR(50),
                @Password NVARCHAR(50)
            AS
            BEGIN
                DECLARE @userId INT
                SET @userId = (
                    SELECT Id
                    FROM dbo.[User]
                    WHERE 
                        Username=@Username AND
                        (CONVERT(NVARCHAR(64), HASHBYTES('SHA2_256', ISNULL(@Password, '') + ISNULL(Salt, '')), 2) = ISNULL(Password, ''))
                )

                SELECT @userId
            END
            ");

            Execute.Sql(@"
            CREATE PROCEDURE IF NOT EXISTS dbo.SetUserCredentials
                @Username nvarchar(50),
                @Password nvarchar(50)
            AS
            BEGIN
                DECLARE @salt NVARCHAR(50)
                DECLARE @userId INT
                
                SET @userId = (SELECT Id
                FROM dbo.[User]
                WHERE Username = @Username)

                IF @userId IS NOT NULL
                BEGIN
                    SELECT @salt = RIGHT(CONVERT(NVARCHAR(64), NEWID()), 12) + RIGHT(CAST(CAST(GETDATE() AS DECIMAL(19,6)) AS NVARCHAR(64)), 10)

                    UPDATE dbo.[User]
                    SET Salt = @salt,
                        Password = CONVERT(NVARCHAR(64), HASHBYTES('SHA2_256', ISNULL(@Password, '') + @salt), 2)
                    WHERE Id = @userId
                END
                
                SELECT @userId
            END
            ");

            Insert.IntoTable("User").Row(new
            {
                Username = "admin",
                Name = "Administrator",
                Password = "",
                Salt = "",
                RegisteredAt = DateTimeOffset.Now
            });

            Execute.Sql("EXEC dbo.SetUserCredentials 'admin', 'admin'");
        }

        public override void Down()
        {
            Execute.Sql("DROP PROCEDURE IF EXISTS dbo.CheckUserCredentials");

            Execute.Sql("DROP PROCEDURE IF EXISTS dbo.SetUserCredentials");
        }
    }
}