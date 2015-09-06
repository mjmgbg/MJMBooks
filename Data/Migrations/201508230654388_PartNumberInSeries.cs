using System.Data.Entity.Migrations;

namespace Data.Migrations
{
    public partial class PartNumberInSeries : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Series", "PartNumber", c => c.Int(false));
        }

        public override void Down()
        {
            DropColumn("dbo.Series", "PartNumber");
        }
    }
}