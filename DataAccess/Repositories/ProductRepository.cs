using System.Linq;
using Customer.Project.DataAccess.RepositoryInterfaces;
using Customer.Project.DataAccess.RepositoryInterfaces.Int;
using Customer.Project.Domain.Entities;
using NHibernate.Linq;

namespace Customer.Project.DataAccess.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository, IProductRepositoryInt 
    {
        #region IProductRepository Members
        
        /// <summary>
        /// Example query method
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public IQueryable<Product> GetProductHavingPriceMoreThan(int price)
        {
            return Session.Query<Product>()
                .Where(x => x.RawPrice > price)
                .OrderBy(x => x.RawPrice);
        }
        #endregion
    }
}
