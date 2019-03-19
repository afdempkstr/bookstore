using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FluentMigrator;

namespace BookStore.Migrations
{
    [Migration(20190320010000)]
    public class AddUserRolesTable : Migration
    {
        public override void Up()
        {
            Create.Table("UserRoles")
                .WithColumn("UserId").AsInt32().NotNullable()
                .ForeignKey("FK_UserRoles_User", "User", "Id")
                .OnDeleteOrUpdate(Rule.Cascade)
                .WithColumn("RoleId").AsInt32().NotNullable()
                .ForeignKey("FK_UserRoles_Role", "Role", "Id")
                .OnDeleteOrUpdate(Rule.Cascade);

            Create.PrimaryKey().OnTable("UserRoles").Columns("UserId", "RoleId");
        }

        public override void Down()
        {
            Delete.Table("UserRoles");
        }
    }
}