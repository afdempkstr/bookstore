using FluentMigrator;

namespace BookStore.Migrations
{
    [Migration(20190306020000)]
    public class AddBookTable : Migration
    {
        public override void Up()
        {
            Create.Table("Book")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Title").AsString(50).NotNullable().Indexed()
                .WithColumn("Author").AsString(50).NotNullable().Indexed()
                .WithColumn("CoverPhoto").AsString(255)
                .WithColumn("PublicationYear").AsInt32().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("Book");
        }
    }
}