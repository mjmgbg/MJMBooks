using System.Data.Entity.Migrations;

namespace Data.Migrations
{
    public partial class AddedNullForSeries : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Books", "SeriesId", "dbo.Series");
            DropForeignKey("dbo.Books", "SeriesPartId", "dbo.SeriesPart");
            DropIndex("dbo.Books", new[] {"SeriesId"});
            DropIndex("dbo.Books", new[] {"SeriesPartId"});
            AlterColumn("dbo.Books", "SeriesId", c => c.Int());
            AlterColumn("dbo.Books", "SeriesPartId", c => c.Int());
            CreateIndex("dbo.Books", "SeriesId");
            CreateIndex("dbo.Books", "SeriesPartId");
            AddForeignKey("dbo.Books", "SeriesId", "dbo.Series", "Id");
            AddForeignKey("dbo.Books", "SeriesPartId", "dbo.SeriesPart", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Books", "SeriesPartId", "dbo.SeriesPart");
            DropForeignKey("dbo.Books", "SeriesId", "dbo.Series");
            DropIndex("dbo.Books", new[] {"SeriesPartId"});
            DropIndex("dbo.Books", new[] {"SeriesId"});
            AlterColumn("dbo.Books", "SeriesPartId", c => c.Int(false));
            AlterColumn("dbo.Books", "SeriesId", c => c.Int(false));
            CreateIndex("dbo.Books", "SeriesPartId");
            CreateIndex("dbo.Books", "SeriesId");
            AddForeignKey("dbo.Books", "SeriesPartId", "dbo.SeriesPart", "Id", true);
            AddForeignKey("dbo.Books", "SeriesId", "dbo.Series", "Id", true);
        }
    }
}