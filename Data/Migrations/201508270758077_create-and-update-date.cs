namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createandupdatedate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "CreateDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Books", "UpdateDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Books", "IsRead", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Books", "IsRead");
            DropColumn("dbo.Books", "UpdateDate");
            DropColumn("dbo.Books", "CreateDate");
        }
    }
}
