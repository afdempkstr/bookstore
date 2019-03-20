using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentMigrator;

namespace BookStore.Migrations
{
    [Migration(20190319010000)]
    public class AddUserTable : Migration
    {
        public override void Up()
        {
            Create.Table("User")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Username").AsString(50).NotNullable().Unique()
                .WithColumn("Name").AsString(50).NotNullable()
                .WithColumn("Password").AsString(255).NotNullable()
                .WithColumn("Salt").AsString(50).NotNullable()
                .WithColumn("RegisteredAt").AsDateTimeOffset().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("User");
        }
    }
}