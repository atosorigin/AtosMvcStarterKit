using Customer.Project.DataAccess.NHibernateHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Customer.Project.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for NHibernateSessionManagerTest and is intended
    ///to contain all NHibernateSessionManagerTest Unit Tests
    ///</summary>
    [TestClass]
    public class NHibernateSessionManagerTest : BaseTest
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
        ///A test for ExportDatabaseSchema
        ///
        ///</summary>
        ///<remarks>
        ///Code Analysis rule can be suppressed because this class is a singleton
        ///</remarks> 
        //[TestMethod()]
        public static void ExportDatabaseSchemaTest()
        {
            NHibernateSessionManager.Instance.InitSessionFactory();
            NHibernateSessionManager.Instance.ExportDatabaseSchema();
        }
    }
}
