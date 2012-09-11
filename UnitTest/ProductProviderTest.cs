using Customer.Project.Application.Providers;
using Customer.Project.DataAccess.RepositoryInterfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Customer.Project.Application;
using Customer.Project.Domain.Entities;
using System.Linq;

namespace Customer.Project.UnitTest
{
    /// <summary>
    ///This is a test class for ProductProviderTest and is intended
    ///to contain all ProductProviderTest Unit Tests
    ///</summary>
    [TestClass]
    public class ProductProviderTest : BaseTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for Save
        ///</summary>
        //[TestMethod]
        public void ShouldCreateIdOnSave()
        {
            ProductProvider target = ServiceLocator.Get<ProductProvider>();
            Product product = new Product {Name = "My new product", RawPrice = 100};
            target.Save(product);
            Assert.IsTrue(product.IdOfEntity != 0);
        }

        //[TestMethod]
        public void GetAllShouldIncludeInsertedProduct()
        {
            var products = ServiceLocator.Get<IProductRepository>().GetAll();
            int count = products.Count();

            // save new product
            ShouldCreateIdOnSave();

            var newProductsList = ServiceLocator.Get<IProductRepository>().GetAll();
            int newCount = newProductsList.Count();

            Assert.IsTrue(newCount == count + 1);
        }

        //[TestMethod]
        public void ShouldRemoveProduct()
        {
            var products = ServiceLocator.Get<IProductRepository>().GetAll();
            int count = products.Count();

            ServiceLocator.Get<ProductProvider>().Delete(products.First());

            int newCount = ServiceLocator.Get<IProductRepository>().GetAll().Count();
            Assert.AreEqual(count - 1, newCount);
        }
    }
}
