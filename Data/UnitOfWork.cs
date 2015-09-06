using Entities;

namespace Data
{
    public class UnitOfWork : DbContextBook, IUnitOfWork
    {
        private readonly GenericRepository<AuthorModel> _authorRepo;
        private readonly BookRepository<BookModel> _bookRepo;
        private readonly GenericRepository<PublisherModel> _publisherRepo;
        private readonly GenericRepository<ReaderModel> _readerRepo;
        private readonly GenericRepository<SeriesModel> _serieRepo;
        private readonly GenericRepository<SeriesPartNumberModel> _seriePartRepo;
        private readonly GenericRepository<LanguageModel> _languageRepo;

        public UnitOfWork()
        {
            _authorRepo = new GenericRepository<AuthorModel>(this);
            _bookRepo = new BookRepository<BookModel>(this);
            _publisherRepo = new GenericRepository<PublisherModel>(this);
            _readerRepo = new GenericRepository<ReaderModel>(this);
            _serieRepo = new GenericRepository<SeriesModel>(this);
            _seriePartRepo = new GenericRepository<SeriesPartNumberModel>(this);
            _languageRepo = new GenericRepository<LanguageModel>(this);
        }

        public IGenericRepository<AuthorModel> AuthorRepository => _authorRepo;

        public IBookRepository<BookModel> BookRepository => _bookRepo;

        public IGenericRepository<PublisherModel> PublisherRepository => _publisherRepo;

        public IGenericRepository<ReaderModel> ReaderRepository => _readerRepo;
        public IGenericRepository<LanguageModel> LanguageRepository => _languageRepo;

        public IGenericRepository<SeriesModel> SerieRepository => _serieRepo;
      
        public IGenericRepository<SeriesPartNumberModel> SeriePartRepository => _seriePartRepo;

        public void Commit()
        {
            SaveChanges();
        }
    }
}