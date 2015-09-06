using System.Data.Entity.Migrations;

namespace Data.Migrations
{
    public partial class SeriesNameIsRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Series", "Name", c => c.String(false));
        }

        public override void Down()
        {
            AlterColumn("dbo.Series", "Name", c => c.String());
        }
    }
}