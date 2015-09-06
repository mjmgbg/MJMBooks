using System.Data.Entity.Migrations;

namespace Data.Migrations
{
    public partial class FixedSeriesColumnNames2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Books", "Series_Id", "dbo.Series");
            DropForeignKey("dbo.Books", "SeriesPart_Id", "dbo.SeriesPart");
            DropIndex("dbo.Books", new[] {"Series_Id"});
            DropIndex("dbo.Books", new[] {"SeriesPart_Id"});
            RenameColumn("dbo.Series", "SeriesId", "Id");
            RenameColumn("dbo.SeriesPart", "SeriesPartId", "Id");
            RenameColumn("dbo.Books", "Series_Id", "SeriesId");
            RenameColumn("dbo.Books", "SeriesPart_Id", "SeriesPartId");
            AlterColumn("dbo.Books", "SeriesId", c => c.Int(false));
            AlterColumn("dbo.Books", "SeriesPartId", c => c.Int(false));
            CreateIndex("dbo.Books", "SeriesId");
            CreateIndex("dbo.Books", "SeriesPartId");
            AddForeignKey("dbo.Books", "SeriesId", "dbo.Series", "Id", true);
            AddForeignKey("dbo.Books", "SeriesPartId", "dbo.SeriesPart", "Id", true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Books", "SeriesPartId", "dbo.SeriesPart");
            DropForeignKey("dbo.Books", "SeriesId", "dbo.Series");
            DropIndex("dbo.Books", new[] {"SeriesPartId"});
            DropIndex("dbo.Books", new[] {"SeriesId"});
            AlterColumn("dbo.Books", "SeriesPartId", c => c.Int());
            AlterColumn("dbo.Books", "SeriesId", c => c.Int());
            RenameColumn("dbo.Books", "SeriesPartId", "SeriesPart_Id");
            RenameColumn("dbo.Books", "SeriesId", "Series_Id");
            RenameColumn("dbo.SeriesPart", "Id", "SeriesPartId");
            RenameColumn("dbo.Series", "Id", "SeriesId");
            CreateIndex("dbo.Books", "SeriesPart_Id");
            CreateIndex("dbo.Books", "Series_Id");
            AddForeignKey("dbo.Books", "SeriesPart_Id", "dbo.SeriesPart", "SeriesPartId");
            AddForeignKey("dbo.Books", "Series_Id", "dbo.Series", "SeriesId");
        }
    }
}