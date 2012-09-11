using Customer.Project.DataAccess.RepositoryInterfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Customer.Project.Domain.Entities;
using Moq;

namespace Customer.Project.UnitTest
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class MockProduct : BaseTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void MockProductMethod()
        {
            //Product newProduct = new Product();
            //Product newProduct = new Product();
            //newProduct.Id=1;
            //newProduct.Name="Bushmills";
            var newProduct = new Mock<Product>();
            //newProduct.Object.ID = 1;
            newProduct.SetupGet(p => p.IdOfEntity).Returns(1);
            //newProduct.Object.Name = "Bushmills";
            newProduct.SetupGet(p => p.Name).Returns("Bushmills");
            //newProduct.ExpectGet(p => p.Id).Returns(1);
            //newProduct.ExpectGet(p => p.Name).Returns("Bushmills");
            Assert.AreEqual("Bushmills", newProduct.Object.Name);
            // Create mocked IProductRepository instance
            var productRepository = new Mock<IProductRepository>();
            // Setup mocked Repository to return mockMOQProduct object for Id = 1
            productRepository.Setup(p => p.Get(1)).Returns(newProduct.Object);
            // Act
            var productReturned = productRepository.Object.Get(1);
            // Assert
            Assert.AreEqual("Bushmills", productReturned.Name);
        }

        [TestMethod]
        public void MockProductMethodGetAny()
        {
            //Product newProduct = new Product();
            //newProduct.Id=1;
            //newProduct.Name="Bushmills";
            var newProduct = new Mock<Product>();
            //newProduct.Object.ID = 1;
            newProduct.SetupGet(p => p.IdOfEntity).Returns(1);
            //newProduct.Object.Name = "Bushmills";
            newProduct.SetupGet(p => p.Name).Returns("Bushmills");
            //newProduct.ExpectGet(p => p.Id).Returns(1);
            //newProduct.ExpectGet(p => p.Name).Returns("Bushmills");
            Assert.AreEqual("Bushmills", newProduct.Object.Name);
            // Create mocked IProductRepository instance
            var productRepository = new Mock<IProductRepository>();
            // Setup mocked Repository to return mockMOQProduct object for Id = 1
            productRepository.Setup(p => p.Get(It.IsAny<long>())).Returns(newProduct.Object);
            // Act
            var productReturned = productRepository.Object.Get(1);
            // Assert
            Assert.AreEqual("Bushmills", productReturned.Name);
        }

        [TestMethod]
        public void MockProductMethodGetWithinRange()
        {
            //Product newProduct = new Product();
            //newProduct.Id=1;
            //newProduct.Name="Bushmills";
            var newProduct = new Mock<Product>();
            //newProduct.Object.ID = 1;
            newProduct.SetupGet(p => p.IdOfEntity).Returns(1);
            //newProduct.Object.Name = "Bushmills";
            newProduct.SetupGet(p => p.Name).Returns("Bushmills");
            //newProduct.ExpectGet(p => p.Id).Returns(1);
            //newProduct.ExpectGet(p => p.Name).Returns("Bushmills");
            Assert.AreEqual("Bushmills", newProduct.Object.Name);
            // Create mocked IProductRepository instance
            var productRepository = new Mock<IProductRepository>();
            // Setup mocked Repository to return mockMOQProduct object for Id = 1
            productRepository.Setup(p => p.Get(It.Is<long>(id => id > 0 && id < 6))).Returns(newProduct.Object);
            // Act
            var productReturned = productRepository.Object.Get(1);
            // Assert
            Assert.AreEqual("Bushmills", productReturned.Name);
        }
        [TestMethod]
        public void MockTaxCalculator()
        {
            //Create a mock with Moq   
            Mock<ITaxCalculator> fakeTaxCalculator = new Mock<ITaxCalculator>();

            // make sure to return 5$ of tax for a 25$ product   
            fakeTaxCalculator.Setup(tax => tax.GetTax(25.0M)).Returns(5.0M);
        }
        [TestMethod]
        public void GetTax()
        {
            //Initialize our product   
            Product myProduct = new Product { IdOfEntity = 1, Name = "Simple Product", RawPrice = 25.0M };

            //Create a mock with Moq   
            Mock<ITaxCalculator> fakeTaxCalculator = new Mock<ITaxCalculator>();

            // make sure to return 5$ of tax for a 25$ product   
            fakeTaxCalculator.Setup(tax => tax.GetTax(25.0M)).Returns(5.0M);

            // Retrived the calculated tax   
            decimal calculatedTax = 0;
            calculatedTax = myProduct.GetPriceWithTax(fakeTaxCalculator.Object);

            // Verify that the "GetTax" method was called from  the interface   
            fakeTaxCalculator.Verify(tax => tax.GetTax(25.0M));

            // Retrived the calculated tax   
            calculatedTax = myProduct.GetPriceWithTax(fakeTaxCalculator.Object);

            // Make sure that the taxes were calculated   
            Assert.AreEqual(calculatedTax, 30.0M);
        }
    }
}
