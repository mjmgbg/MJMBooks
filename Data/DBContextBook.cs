using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Entities;

namespace Data
{
    public class DbContextBook : DbContext
    {
        public DbContextBook()
            : base("name=MJMBooks")
        {
            Configuration.LazyLoadingEnabled = false;
            //Configuration.AutoDetectChangesEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public virtual DbSet<AuthorModel> Authors { get; set; }
        public virtual DbSet<BookModel> Books { get; set; }
        public virtual DbSet<GenreModel> Genres { get; set; }
        public virtual DbSet<LanguageModel> Languages { get; set; }
        public virtual DbSet<PublisherModel> Publishers { get; set; }
        public virtual DbSet<ReaderModel> Readers { get; set; }
        public virtual DbSet<SeriesPartNumberModel> SeriesPart { get; set; }
        public virtual DbSet<SeriesModel> Series { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<AuthorModel>().ToTable("Authors");
            modelBuilder.Entity<BookModel>().ToTable("Books");
            modelBuilder.Entity<GenreModel>().ToTable("Genres");
            modelBuilder.Entity<LanguageModel>().ToTable("Languages");
            modelBuilder.Entity<ReaderModel>().ToTable("Readers");
            modelBuilder.Entity<SeriesModel>().ToTable("Series");
            modelBuilder.Entity<SeriesPartNumberModel>().ToTable("SeriesPart");

            modelBuilder.Entity<BookModel>().Property(b => b.Title).IsRequired();
            modelBuilder.Entity<BookModel>().Property(b => b.Isbn).IsRequired();
            modelBuilder.Entity<BookModel>().Property(b => b.Description).IsRequired();

            modelBuilder.Entity<SeriesModel>().Property(s => s.Name).IsRequired();

            modelBuilder.Entity<BookModel>()
                .HasMany(b => b.Readers)
                .WithMany(r => r.Books).
                Map(
                    m =>
                    {
                        m.MapLeftKey("BookId");
                        m.MapRightKey("ReaderId");
                        m.ToTable("ReadersInBook");
                    });

            modelBuilder.Entity<BookModel>()
                .HasMany(b => b.Authors)
                .WithMany(a => a.Books).
                Map(
                    m =>
                    {
                        m.MapLeftKey("BookId");
                        m.MapRightKey("AuthorId");
                        m.ToTable("AuthorsInBook");
                    });
            modelBuilder.Entity<BookModel>()
                .HasMany(b => b.Genres)
                .WithMany(g => g.Books).
                Map(
                    m =>
                    {
                        m.MapLeftKey("BookId");
                        m.MapRightKey("GenreId");
                        m.ToTable("GenresInBook");
                    });

            modelBuilder.Entity<LanguageModel>()
                .HasMany(e => e.Books)
                .WithRequired(e => e.Language)
                .HasForeignKey(e => e.LanguageId);

            modelBuilder.Entity<PublisherModel>()
                .HasMany(e => e.Books)
                .WithRequired(e => e.Publisher)
                .HasForeignKey(e => e.PublisherId);
        }
    }
}