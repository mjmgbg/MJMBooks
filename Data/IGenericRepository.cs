using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Data
{
    public interface IGenericRepository<T>
        where T : class
    {
        IQueryable<T> AsQueryable();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        List<T> GetAll();
        T GetById(int id);
        void InsertOrUpdate(T entity);
        void Delete(int id);
    }
}