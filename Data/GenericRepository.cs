using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Entities;

namespace Data
{
    public class GenericRepository<T> : IGenericRepository<T>, IDisposable
        where T : class, IBaseObject
    {
        protected DbContextBook Context;

        public GenericRepository(DbContextBook context)
        {
            Context = context;
        }

        public IQueryable<T> AsQueryable()
        {
            var dbSet = Context.Set<T>();
            return dbSet.AsQueryable();
        }

        public virtual List<T> GetAll()
        {
            var dbSet = Context.Set<T>();
            return dbSet.ToList();
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            var dbSet = Context.Set<T>();
            return dbSet.Where(predicate);
        }

        public virtual T GetById(int id)
        {
            var dbSet = Context.Set<T>();
            return dbSet.Find(id);
        }


        public void InsertOrUpdate(T entity)
        {
            if (entity.Id == default(int)) // New entity
            {
                Context.Entry(entity).State = EntityState.Added;
            }
            else // Existing entity
            {
                Context.Entry(entity).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var dbSet = Context.Set<T>();
            var entity = dbSet.Find(id);
            dbSet.Remove(entity);
        }

        public void Dispose()
        {
            if (Context == null) return;
            Context.Dispose();
            Context = null;
        }
    }
}