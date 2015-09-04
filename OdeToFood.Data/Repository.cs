using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using OddToFood.Contracts;

namespace OdeToFood.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {

        protected OdeToFoodContext Context;

        public Repository(DbContext context)
        {
            Context = (OdeToFoodContext)context;
        }

        public ICollection<T> GetAll()
        {

            return Context.Set<T>().ToList();
        }

        public T Get(int id)
        {
            return Context.Set<T>().Find(id);
        }

        public T Find(Expression<Func<T, bool>> match)
        {
            return Context.Set<T>().SingleOrDefault(match);
        }

        public ICollection<T> FindAll(Expression<Func<T, bool>> match)
        {
            return Context.Set<T>().Where(match).ToList();
        }

        public T Add(T t)
        {
            Context.Set<T>().Add(t);
            Context.SaveChanges();
            return t;
        }

        public T Update(T updated, int key)
        {
            if (updated == null)
            {
                return null;
            }

            T existing = Context.Set<T>().Find(key);
            if (existing != null)
            {
                Context.Entry(existing).CurrentValues.SetValues(updated);
                Context.SaveChanges();
            }
            return existing;
        }

        public virtual void Delete(int key)
        {
            T objDelete = Context.Set<T>().Find(key);
            if (objDelete != null)
            {
                Context.Set<T>().Remove(objDelete);
                Context.SaveChanges();
            }
        }
    }
}
