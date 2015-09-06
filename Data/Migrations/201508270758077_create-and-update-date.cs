using System.Data.Entity.Migrations;

namespace Data.Migrations
{
    public partial class Createandupdatedate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "CreateDate", c => c.DateTime(false));
            AddColumn("dbo.Books", "UpdateDate", c => c.DateTime(false));
            AddColumn("dbo.Books", "IsRead", c => c.Boolean(false));
        }

        public override void Down()
        {
            DropColumn("dbo.Books", "IsRead");
            DropColumn("dbo.Books", "UpdateDate");
            DropColumn("dbo.Books", "CreateDate");
        }
    }
}