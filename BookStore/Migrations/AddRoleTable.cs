using FluentMigrator;

namespace BookStore.Migrations
{
    [Migration(20190318010000)]
    public class AddRolesTable : Migration
    {
        public override void Up()
        {
            Create.Table("Role")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Name").AsString(50).NotNullable().Unique();

            Insert.IntoTable("Role").Row(new {Name = "Customer"});
            Insert.IntoTable("Role").Row(new {Name = "Employee"});
            Insert.IntoTable("Role").Row(new { Name = "Admin" });
        }

        public override void Down()
        {
            Delete.Table("Role");
        }
    }
}