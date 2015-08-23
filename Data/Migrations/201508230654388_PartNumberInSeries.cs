namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PartNumberInSeries : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Series", "PartNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Series", "PartNumber");
        }
    }
}
