using FluentMigrator;

namespace BookStore.Migrations
{
    [Migration(20190306010000)]
    public class AddPublisherTable : Migration
    {
        public override void Up()
        {
            Create.Table("Publisher")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Name").AsString(50).NotNullable().Indexed();
        }

        public override void Down()
        {
            Delete.Table("Publisher");
        }
    }
}