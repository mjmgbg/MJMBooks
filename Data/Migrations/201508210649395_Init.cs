using System.Data.Entity.Migrations;

namespace Data.Migrations
{
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Authors",
                c => new
                {
                    Id = c.Int(false, true),
                    FirstName = c.String(),
                    LastName = c.String()
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Books",
                c => new
                {
                    Id = c.Int(false, true),
                    Title = c.String(false),
                    ImagePath = c.String(false),
                    ISBN = c.String(false),
                    LanguageId = c.Int(false),
                    PublishingDate = c.DateTime(false),
                    PublisherId = c.Int(false),
                    Description = c.String(false),
                    BgColor = c.String(),
                    TextColor = c.String(),
                    TextColorSecond = c.String(),
                    Series_Id = c.Int()
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Languages", t => t.LanguageId, true)
                .ForeignKey("dbo.PublisherModel", t => t.PublisherId, true)
                .ForeignKey("dbo.Series", t => t.Series_Id)
                .Index(t => t.LanguageId)
                .Index(t => t.PublisherId)
                .Index(t => t.Series_Id);

            CreateTable(
                "dbo.Genres",
                c => new
                {
                    Id = c.Int(false, true),
                    Name = c.String()
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Languages",
                c => new
                {
                    Id = c.Int(false, true),
                    Name = c.String()
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.PublisherModel",
                c => new
                {
                    Id = c.Int(false, true),
                    Name = c.String()
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Readers",
                c => new
                {
                    Id = c.Int(false, true),
                    FirstName = c.String(),
                    LastName = c.String()
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Series",
                c => new
                {
                    Id = c.Int(false, true),
                    Name = c.String()
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Origins",
                c => new
                {
                    Id = c.Int(false, true),
                    Name = c.String(),
                    Origin = c.String()
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.AuthorsInBook",
                c => new
                {
                    BookId = c.Int(false),
                    AuthorId = c.Int(false)
                })
                .PrimaryKey(t => new {t.BookId, t.AuthorId})
                .ForeignKey("dbo.Books", t => t.BookId, true)
                .ForeignKey("dbo.Authors", t => t.AuthorId, true)
                .Index(t => t.BookId)
                .Index(t => t.AuthorId);

            CreateTable(
                "dbo.GenresInBook",
                c => new
                {
                    BookId = c.Int(false),
                    GenreId = c.Int(false)
                })
                .PrimaryKey(t => new {t.BookId, t.GenreId})
                .ForeignKey("dbo.Books", t => t.BookId, true)
                .ForeignKey("dbo.Genres", t => t.GenreId, true)
                .Index(t => t.BookId)
                .Index(t => t.GenreId);

            CreateTable(
                "dbo.ReadersInBook",
                c => new
                {
                    BookId = c.Int(false),
                    ReaderId = c.Int(false)
                })
                .PrimaryKey(t => new {t.BookId, t.ReaderId})
                .ForeignKey("dbo.Books", t => t.BookId, true)
                .ForeignKey("dbo.Readers", t => t.ReaderId, true)
                .Index(t => t.BookId)
                .Index(t => t.ReaderId);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Books", "Series_Id", "dbo.Series");
            DropForeignKey("dbo.ReadersInBook", "ReaderId", "dbo.Readers");
            DropForeignKey("dbo.ReadersInBook", "BookId", "dbo.Books");
            DropForeignKey("dbo.Books", "PublisherId", "dbo.PublisherModel");
            DropForeignKey("dbo.Books", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.GenresInBook", "GenreId", "dbo.Genres");
            DropForeignKey("dbo.GenresInBook", "BookId", "dbo.Books");
            DropForeignKey("dbo.AuthorsInBook", "AuthorId", "dbo.Authors");
            DropForeignKey("dbo.AuthorsInBook", "BookId", "dbo.Books");
            DropIndex("dbo.ReadersInBook", new[] {"ReaderId"});
            DropIndex("dbo.ReadersInBook", new[] {"BookId"});
            DropIndex("dbo.GenresInBook", new[] {"GenreId"});
            DropIndex("dbo.GenresInBook", new[] {"BookId"});
            DropIndex("dbo.AuthorsInBook", new[] {"AuthorId"});
            DropIndex("dbo.AuthorsInBook", new[] {"BookId"});
            DropIndex("dbo.Books", new[] {"Series_Id"});
            DropIndex("dbo.Books", new[] {"PublisherId"});
            DropIndex("dbo.Books", new[] {"LanguageId"});
            DropTable("dbo.ReadersInBook");
            DropTable("dbo.GenresInBook");
            DropTable("dbo.AuthorsInBook");
            DropTable("dbo.Origins");
            DropTable("dbo.Series");
            DropTable("dbo.Readers");
            DropTable("dbo.PublisherModel");
            DropTable("dbo.Languages");
            DropTable("dbo.Genres");
            DropTable("dbo.Books");
            DropTable("dbo.Authors");
        }
    }
}