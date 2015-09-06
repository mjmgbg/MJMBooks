using System.Data.Entity.Migrations;

namespace Data.Migrations
{
    public partial class ChangedSeriesPart : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SeriesPart", "Name", c => c.String());
            DropColumn("dbo.SeriesPart", "PartNumber");
        }

        public override void Down()
        {
            AddColumn("dbo.SeriesPart", "PartNumber", c => c.Int(false));
            DropColumn("dbo.SeriesPart", "Name");
        }
    }
}