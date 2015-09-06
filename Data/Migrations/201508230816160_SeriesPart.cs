using System.Data.Entity.Migrations;

namespace Data.Migrations
{
    public partial class SeriesPart : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SeriesPart",
                c => new
                {
                    Id = c.Int(false, true),
                    SeriesId = c.Int(false),
                    PartNumber = c.Int(false)
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.Books", "SeriesPart_Id", c => c.Int());
            CreateIndex("dbo.Books", "SeriesPart_Id");
            AddForeignKey("dbo.Books", "SeriesPart_Id", "dbo.SeriesPart", "Id");
            DropColumn("dbo.Series", "PartNumber");
        }

        public override void Down()
        {
            AddColumn("dbo.Series", "PartNumber", c => c.Int(false));
            DropForeignKey("dbo.Books", "SeriesPart_Id", "dbo.SeriesPart");
            DropIndex("dbo.Books", new[] {"SeriesPart_Id"});
            DropColumn("dbo.Books", "SeriesPart_Id");
            DropTable("dbo.SeriesPart");
        }
    }
}