using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FluentMigrator;

namespace BookStore.Migrations
{
    [Migration(201903090000000)]
    public class AddBookPublisherForeignKey : Migration
    {
        public override void Up()
        {
            Alter.Table("Book")
                .AddColumn("PublisherId").AsInt32().NotNullable()
                .ForeignKey("Publisher", "Id")
                .OnDeleteOrUpdate(Rule.Cascade);
        }

        public override void Down()
        {
            Delete.ForeignKey().FromTable("Publisher");
            Delete.Column("PublisherId").FromTable("Book");
        }
    }
}