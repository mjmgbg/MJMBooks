namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixedSeriesColumnNames2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Books", "Series_Id", "dbo.Series");
            DropForeignKey("dbo.Books", "SeriesPart_Id", "dbo.SeriesPart");
            DropIndex("dbo.Books", new[] { "Series_Id" });
            DropIndex("dbo.Books", new[] { "SeriesPart_Id" });
            RenameColumn(table: "dbo.Series", name: "SeriesId", newName: "Id");
            RenameColumn(table: "dbo.SeriesPart", name: "SeriesPartId", newName: "Id");
            RenameColumn(table: "dbo.Books", name: "Series_Id", newName: "SeriesId");
            RenameColumn(table: "dbo.Books", name: "SeriesPart_Id", newName: "SeriesPartId");
            AlterColumn("dbo.Books", "SeriesId", c => c.Int(nullable: false));
            AlterColumn("dbo.Books", "SeriesPartId", c => c.Int(nullable: false));
            CreateIndex("dbo.Books", "SeriesId");
            CreateIndex("dbo.Books", "SeriesPartId");
            AddForeignKey("dbo.Books", "SeriesId", "dbo.Series", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Books", "SeriesPartId", "dbo.SeriesPart", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Books", "SeriesPartId", "dbo.SeriesPart");
            DropForeignKey("dbo.Books", "SeriesId", "dbo.Series");
            DropIndex("dbo.Books", new[] { "SeriesPartId" });
            DropIndex("dbo.Books", new[] { "SeriesId" });
            AlterColumn("dbo.Books", "SeriesPartId", c => c.Int());
            AlterColumn("dbo.Books", "SeriesId", c => c.Int());
            RenameColumn(table: "dbo.Books", name: "SeriesPartId", newName: "SeriesPart_Id");
            RenameColumn(table: "dbo.Books", name: "SeriesId", newName: "Series_Id");
            RenameColumn(table: "dbo.SeriesPart", name: "Id", newName: "SeriesPartId");
            RenameColumn(table: "dbo.Series", name: "Id", newName: "SeriesId");
            CreateIndex("dbo.Books", "SeriesPart_Id");
            CreateIndex("dbo.Books", "Series_Id");
            AddForeignKey("dbo.Books", "SeriesPart_Id", "dbo.SeriesPart", "SeriesPartId");
            AddForeignKey("dbo.Books", "Series_Id", "dbo.Series", "SeriesId");
        }
    }
}
