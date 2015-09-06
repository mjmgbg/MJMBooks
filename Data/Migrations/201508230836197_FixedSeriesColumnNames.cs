using System.Data.Entity.Migrations;

namespace Data.Migrations
{
    public partial class FixedSeriesColumnNames : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.Series", "Id", "SeriesId");
            RenameColumn("dbo.SeriesPart", "Id", "SeriesPartId");
        }

        public override void Down()
        {
            RenameColumn("dbo.SeriesPart", "SeriesPartId", "Id");
            RenameColumn("dbo.Series", "SeriesId", "Id");
        }
    }
}