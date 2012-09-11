using System;
using System.Linq;
using Customer.Project.DataAccess.RepositoryInterfaces;
using Customer.Project.DataAccess.RepositoryInterfaces.Int;
using Customer.Project.Domain.Entities;

namespace InMemoryTestData.Repositories
{
    public class ProductRepository : BaseInMemoryRepository<Product>, IProductRepository, IProductRepositoryInt 
    {
        #region IProductRepository Members

        public IQueryable<Product> GetProductHavingPriceMoreThan(int price)
        {
            return (from p in GetAll()
                    where p.RawPrice > price
                    select p);
        }
        #endregion
    }
}
