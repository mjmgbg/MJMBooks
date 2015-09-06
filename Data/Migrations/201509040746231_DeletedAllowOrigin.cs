using System.Data.Entity.Migrations;

namespace Data.Migrations
{
    public partial class DeletedAllowOrigin : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Origins");
        }

        public override void Down()
        {
            CreateTable(
                "dbo.Origins",
                c => new
                {
                    Id = c.Int(false, true),
                    Name = c.String(),
                    Origin = c.String()
                })
                .PrimaryKey(t => t.Id);
        }
    }
}