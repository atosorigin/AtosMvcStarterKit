using System.IO;
using Customer.Project.Application;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Customer.Project.DataAccess.NHibernateHelpers;

namespace Customer.Project.UnitTest
{
    /// <summary>
    /// Base class for all test classes
    /// </summary>
    [TestClass]
    [DeploymentItem("Mvc\\NHibernate.config")]
    [DeploymentItem("Mvc\\log4net.config")]
    public class BaseTest
    {
        /// <summary>
        /// Set mUseNHibernate to true to perform unit tests on nhibernate dataaccess repositories
        /// Set mUseNHibernate to false to perform unit tests on InMemoryTestData repositories
        /// </summary>
        private const bool UseNhibernate = false;

        private static bool _initialized = false;

        private static object _unitTestSession;

        [TestInitialize]
        public virtual void BaseTestInit()
        {
            if (!_initialized)
            {
                NHibernateSessionManager.GetUnitTestSession +=
                    () => _unitTestSession;

                NHibernateSessionManager.SetUnitTestSession +=
                    o => _unitTestSession = o;

                //Tell the application to use log4net and get config settings from the mentioned path.
                log4net.Config.XmlConfigurator.Configure(new FileInfo("log4net.config"));

                _initialized = true;
            }

            ServiceLocator.InitializeContainer(null);

            // for each test start transaction
            if (UseNhibernate)
            {
                NHibernateSessionManager.Instance.BeginTransaction();
            }
        }
        
        [TestCleanup]
        public  void BaseTestCleanup()
        {
            if (UseNhibernate)
            {
                //NHibernateSessionManager.Instance.CommitTransaction();
                NHibernateSessionManager.Instance.RollbackTransaction();
                NHibernateSessionManager.Instance.CloseSession();
            }
        }
    }
}
