namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixedSeriesColumnNames : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Series", name: "Id", newName: "SeriesId");
            RenameColumn(table: "dbo.SeriesPart", name: "Id", newName: "SeriesPartId");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.SeriesPart", name: "SeriesPartId", newName: "Id");
            RenameColumn(table: "dbo.Series", name: "SeriesId", newName: "Id");
        }
    }
}
