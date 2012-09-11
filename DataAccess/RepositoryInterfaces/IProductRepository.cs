using System.Linq;
using Customer.Project.Domain.Entities;

namespace Customer.Project.DataAccess.RepositoryInterfaces
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        IQueryable<Product> GetProductHavingPriceMoreThan(int price);
    }    
}
