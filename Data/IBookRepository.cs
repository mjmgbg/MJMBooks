using System.Collections.Generic;
using Business.DTO;
using Entities;

namespace Data
{
    public interface IBookRepository<T> : IGenericRepository<T>
        where T : class, IBaseObject
    {
        List<BookViewModel> ConvertDbToModel(List<T> dbBook);
        BookViewModel ConvertDbToModel(T dbBook);
        T ConvertModelToDb(BookViewModel modelBook);
        bool IsBookInDb(string isbn);
        void AddFromAdlibris(string connectionString, BookDetailDto dto);
    }
}