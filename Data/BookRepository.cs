using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Business;
using Business.DTO;
using Entities;

namespace Data
{
    public class BookRepository<T> : GenericRepository<T>, IBookRepository<T>
        where T : class, IBaseObject
    {
        public BookRepository(DbContextBook context)
            : base(context)
        {
        }

        public override List<T> GetAll()
        {
            return
                Context.Books.Include(b => b.Authors).Include(b => b.Readers).Include(b => b.Series).ToList() as List<T>;
        }

        public override T GetById(int id)
        {
            return
                Context.Books.Include(b => b.Authors)
                    .Include(b => b.Readers)
                    .Include(b => b.Series)
                    .FirstOrDefault(b => b.Id == id) as T;
        }


        public bool IsBookInDb(string isbn)
        {
            var book = Context.Books.FirstOrDefault(b => b.Isbn == isbn);
            return book != null;
        }

        public BookViewModel ConvertDbToModel(T dbBook)
        {
            var book = GetById(dbBook.Id) as BookModel;
            if (book == null) return null;
            var authors = Converters.ConvertPersonDbToModel(book.Authors.ToList());
            var readers = Converters.ConvertPersonDbToModel(book.Readers.ToList());
            var publisher = Converters.ConvertLookUpDbToModel(book.Publisher);
            var serie = Converters.ConvertLookUpDbToModel(book.Series);
            var seriePart = Converters.ConvertLookUpDbToModel(book.SeriesPart);
            return new BookViewModel
            {
                Id = book.Id,
                LanguageId = book.LanguageId,
                PublisherId = book.PublisherId,
                SeriesId = book.SeriesId,
                SeriesPartId = book.SeriesPartId,
                Title = book.Title,
                Description = book.Description,
                Isbn = book.Isbn,
                ImagePath = book.ImagePath,
                BgColor = book.BgColor,
                TextColor = book.TextColor,
                TextColorSecond = book.TextColorSecond,
                IsRead = book.IsRead,
                Authors = authors,
                Readers = readers,
                Publisher = publisher,
                Series = serie,
                SeriesPart = seriePart,
                PublishingDate = book.PublishingDate,
                CreateDate = book.CreateDate,
                UpdateDate = book.UpdateDate
            };
        }

        public List<BookViewModel> ConvertDbToModel(List<T> dbBook)
        {
            return dbBook.Select(ConvertDbToModel).ToList();
        }

        public T ConvertModelToDb(BookViewModel modelBook)
        {
            var book = GetById(modelBook.Id) as BookModel ?? new BookModel();

            book.Authors.Clear();
            book.Readers.Clear();
            var authorList = SetSelectedPersonsToList<AuthorModel>(modelBook.AuthorsList);
            var readerList = SetSelectedPersonsToList<ReaderModel>(modelBook.ReadersList);
            var language = Context.Languages.Find(modelBook.LanguageId);
            var publisher = Context.Publishers.Find(modelBook.PublisherId);
            var serie = Context.Series.Find(modelBook.SeriesId);
            var seriePart = Context.SeriesPart.Find(modelBook.SeriesPartId);

            book.LanguageId = modelBook.LanguageId;
            book.PublisherId = modelBook.PublisherId;
            book.SeriesId = modelBook.SeriesId;
            book.SeriesPartId = modelBook.SeriesPartId;
            book.Publisher = publisher;
            book.Language = language;
            book.Readers = readerList;
            //book.Authors = authorList;
            book.Genres = null;
            book.Series = serie;
            book.SeriesPart = seriePart;
            book.Title = modelBook.Title;
            book.Description = modelBook.Description;
            book.Isbn = modelBook.Isbn;
            book.ImagePath = modelBook.ImagePath;
            book.BgColor = modelBook.BgColor;
            book.TextColor = modelBook.TextColor;
            book.TextColorSecond = modelBook.TextColorSecond;
            book.IsRead = modelBook.IsRead;
            book.PublishingDate = modelBook.PublishingDate;
            book.CreateDate = modelBook.CreateDate;
            book.UpdateDate = modelBook.UpdateDate;
            return book as T;
        }


        private List<T1> SetSelectedPersonsToList<T1>(int[] modelList)
            where T1 : class

        {
            var dbSet = Context.Set<T1>();
            return modelList.Select(item => dbSet.Find(item)).Where(entity => entity != null).ToList();
        }

        public void AddFromAdlibris(string connectionString, BookDetailDto dto)
        {
            AdlibrisHelper.Context = Context;
            AdlibrisHelper.AddFromAdlibris(connectionString,dto);
        }
    }
}