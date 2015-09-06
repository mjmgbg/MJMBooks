using System;
using Entities;

namespace Data
{
    public interface IUnitOfWork : IDisposable
    {
        IBookRepository<BookModel> BookRepository { get; }
        IGenericRepository<AuthorModel> AuthorRepository { get; }
        IGenericRepository<PublisherModel> PublisherRepository { get; }
        IGenericRepository<ReaderModel> ReaderRepository { get; }
        IGenericRepository<SeriesModel> SerieRepository { get; }

        void Commit();
    }
}