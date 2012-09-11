using Customer.Project.DataAccess.RepositoryInterfaces.Int;
using Customer.Project.Domain.Entities;
using AtosOrigin.NetLibrary.Components.Core;

namespace Customer.Project.Application.Providers
{
    public class ProductProvider
    {
        private IProductRepositoryInt _productRepository;

        public ProductProvider(IProductRepositoryInt repository)
        {
            _productRepository = repository;
        }
        public virtual void Save(Product product)
        {
            Check.Require(product != null, "Product is required for Save");
            _productRepository.SaveOrUpdate(product);
        }

        public virtual void Delete(Product product)
        {
            Check.Require(product != null, "Product is required for Delete");
            _productRepository.Delete(product);
        }

    }

}
