namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeriesPart : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SeriesPart",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SeriesId = c.Int(nullable: false),
                        PartNumber = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Books", "SeriesPart_Id", c => c.Int());
            CreateIndex("dbo.Books", "SeriesPart_Id");
            AddForeignKey("dbo.Books", "SeriesPart_Id", "dbo.SeriesPart", "Id");
            DropColumn("dbo.Series", "PartNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Series", "PartNumber", c => c.Int(nullable: false));
            DropForeignKey("dbo.Books", "SeriesPart_Id", "dbo.SeriesPart");
            DropIndex("dbo.Books", new[] { "SeriesPart_Id" });
            DropColumn("dbo.Books", "SeriesPart_Id");
            DropTable("dbo.SeriesPart");
        }
    }
}
