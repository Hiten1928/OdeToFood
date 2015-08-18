using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OdeToFood.Core;
using System.Linq.Expressions;
using OddToFood.Contracts;
using System.Data.Entity;

namespace OdeToFood.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {

        protected OdeToFoodContext _context;

        public Repository(DbContext context)
        {
            _context = (OdeToFoodContext)context;
        }

        public ICollection<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public T Get(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public T Find(Expression<Func<T, bool>> match)
        {
            return _context.Set<T>().SingleOrDefault(match);
        }

        public ICollection<T> FindAll(Expression<Func<T, bool>> match)
        {
            return _context.Set<T>().Where(match).ToList();
        }

        public T Add(T t)
        {
            _context.Set<T>().Add(t);
            _context.SaveChanges();
            return t;
        }

        public T Update(T updated, int key)
        {
            if (updated == null)
                return null;

            T existing = _context.Set<T>().Find(key);
            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(updated);
                _context.SaveChanges();
            }
            return existing;
        }

        public void Delete(int key)
        {
            T objDelete = _context.Set<T>().Find(key);
            if (objDelete != null)
            {
                _context.Set<T>().Remove(objDelete);
                _context.SaveChanges();
            }
        }
    }
}
