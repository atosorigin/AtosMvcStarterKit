using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Customer.Project.DataAccessEF.Repositories
{
    public partial class Repository<T> where T : class
    {
        protected readonly MvcContext _context = new MvcContext();

        public IQueryable<T> All
        {
            get { return _context.Set<T>(); }
        }

        public IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        /// <summary>
        /// Search for a single item
        /// </summary>
        public T Find(object id)
        {
            return _context.Set<T>().Find(id);
        }

        public void Delete(object id)
        {
            var item = Find(id);
            if (null != item)
            {
                _context.Set<T>().Remove(item);
            }
        }

        /// <summary>
        /// Apply database context changes to the database
        /// </summary>
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}