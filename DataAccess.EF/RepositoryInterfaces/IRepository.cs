using System;
using System.Linq;
using System.Linq.Expressions;

namespace Customer.Project.DataAccessEF.RepositoryInterfaces
{
    public interface IRepository<T>
    {
        IQueryable<T> All { get; }
        IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
        T Find(object id);
        void Delete(object id);
        void Save();
    }
}
