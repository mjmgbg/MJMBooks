namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeriesNameIsRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Series", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Series", "Name", c => c.String());
        }
    }
}
