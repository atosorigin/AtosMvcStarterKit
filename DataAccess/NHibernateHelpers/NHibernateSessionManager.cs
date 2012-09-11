using System;
using System.Data.SqlClient;
using System.Security.Permissions;
using System.Reflection;
using System.IO;
using System.Runtime.Remoting.Messaging;
using NHibernate;
using NHibernate.Cache;
using System.Web;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using FluentNHibernate.Cfg;
using FluentNHibernate.Automapping;
using Customer.Project.DataAccess.NHibernateMappings;
using Customer.Project.Domain.Entities;
using AtosOrigin.NetLibrary.Components.Core;

namespace Customer.Project.DataAccess.NHibernateHelpers
{
    /// <summary>
    /// Handles creation and management of sessions and transactions.  It is a singleton because 
    /// building the initial session factory is very expensive. Inspiration for this class came 
    /// from Chapter 8 of Hibernate in Action by Bauer and King.  Although it is a sealed singleton
    /// you can use TypeMock (http://www.typemock.com) for more flexible testing.
    /// </summary>
    public class NHibernateSessionManager
    {
        private ISessionFactory mSessionFactory = null;
        private Configuration mConfiguration = null;

        private const string TransactionKey = "CTX_TRANSACTION";
        private const string SessionKey = "CTX_SESSION";

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static object UnitTestSession;
        public delegate object GetUnitTestSessionFunc();
        public delegate void SetUnitTestSessionFunc(object session);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static GetUnitTestSessionFunc GetUnitTestSession;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static SetUnitTestSessionFunc SetUnitTestSession;

        /// <summary>
        /// Set to true if anything in the session has been saved. Only if this flag is set, data 
        /// will be stored to the database at the end of the session
        /// </summary>
        public bool AutoCommitTransaction { get; set; }

        /// <summary>
        /// Set to true to cancel the transaction. This flag overrules CommitTransaction
        /// </summary>
        public bool CancelTransaction { get; set; }

        #region Thread-safe, lazy Singleton

        /// <summary>
        /// This is a thread-safe, lazy singleton.  See http://www.yoda.arachsys.com/csharp/singleton.html
        /// for more details about its implementation.
        /// </summary>
        public static NHibernateSessionManager Instance
        {
            get
            {
                return Nested.NHibernateSessionManager;
            }
        }

        /// <summary>
        /// Assists with ensuring thread-safe, lazy singleton
        /// </summary>
        private class Nested
        {
            private Nested() { }

            internal static readonly NHibernateSessionManager NHibernateSessionManager = new NHibernateSessionManager();
        }


        /// <summary>
        /// Initializes the NHibernate session factory upon instantiation.
        /// </summary>
        private NHibernateSessionManager()
        {
            mConfiguration = null;
            mSessionFactory = null;
            CancelTransaction = false;
            AutoCommitTransaction = false;
        }

        #endregion

        #region Initialization and cleanup

        /// <summary>
        /// Inits the session factory.
        /// </summary>
        public void InitSessionFactory(string xmlConfigFile)
        {
            if (mSessionFactory == null)
            {
                // enabling the reflection optimizer inflicts a startup cost but improves the query performance
                NHibernate.Cfg.Environment.UseReflectionOptimizer = true;
                mConfiguration = new Configuration()
                       .Configure(xmlConfigFile);
                mSessionFactory = AutoMappings();
            }
        }
        public void InitSessionFactory()
        {
            // make sure that the session factory is initialized for only one ASP.NET request
            // subsequent requests must wait until the first one has initialized the mSessionFactory
            lock (this)
            {
                if (mSessionFactory == null)
                {
                    if (null != HttpContext.Current)
                    {
                        // Initialize NHibernate for the web application
                        string xmlConfigFile = HttpContext.Current.Server.MapPath("~/NHibernate.config");
                        mConfiguration = new Configuration()
                            .Configure(xmlConfigFile);
                    }
                    else
                    {
                        // Initialize NHibernate for unit tests
                        var xmlConfigFile = Path.GetDirectoryName(
                            Assembly.GetExecutingAssembly().CodeBase) + @"\NHibernate.config";
                        mConfiguration = new Configuration()
                            .Configure(xmlConfigFile);
                    }

                    mSessionFactory = AutoMappings();
                }
            }
        }
        private ISessionFactory AutoMappings()
        {
            var autoMappingConfig = new CustomAutoMappingConfiguration();
            var configuration = Fluently.Configure(mConfiguration)
                .Mappings(m =>
                {
                    m.AutoMappings.Add(
                        AutoMap.AssemblyOf<Product>(autoMappingConfig)
                            .UseOverridesFromAssemblyOf<ProductMap>()
                        // use .IncludeBase<YourBaseClass>() to map base entity classes
                            .Conventions.Add<StringToVarCharPropertyConvention>()                            
                        );
                })
                .BuildSessionFactory();

            return configuration;
        }

        /// <summary>
        /// Closes the session and the disposes the session factory
        /// </summary>
        public void CloseSessionFactory()
        {
            CloseSession();
            mSessionFactory.Close();
            mSessionFactory.Dispose();
            mSessionFactory = null;
        }

        #endregion

        #region Session Management

        /// <summary>
        /// Gets or sets the thread session.
        /// </summary>
        /// <value>The thread session.</value>
        private static ISession ContextSession
        {
            get
            {
                if (IsInWebContext())
                {
                    return (ISession)HttpContext.Current.Items[SessionKey];
                }

                Check.Require(GetUnitTestSession != null
                    , "GetUnitTestSession must be implemented. Does your unit test class derive from BaseTest?");

                return (ISession)GetUnitTestSession();
            }
            set
            {
                if (IsInWebContext())
                {
                    HttpContext.Current.Items[SessionKey] = value;
                }
                else
                {
                    Check.Require(SetUnitTestSession != null);
                    SetUnitTestSession(value);
                }
            }
        }

        /// <summary>
        /// Allows you to register an interceptor on a new session.  This may not be called if there is already
        /// an open session attached to the HttpContext.  If you have an interceptor to be used, modify
        /// the HttpModule to call this before calling BeginTransaction().
        /// </summary>
        public void RegisterInterceptor(IInterceptor interceptor)
        {
            if (ContextSession != null && ContextSession.IsOpen)
            {
                throw new CacheException("You cannot register an interceptor once a session has already been opened");
            }

            GetSession(interceptor);
        }

        public ISession OpenNewSession()
        {
            if (mSessionFactory == null)
            {
                InitSessionFactory();
            }
            return mSessionFactory.OpenSession();
        }

        /// <summary>
        /// Gets the session.
        /// </summary>
        /// <returns></returns>
        public ISession GetSession()
        {
            return GetSession(null);
        }

        /// <summary>
        /// Gets a session with or without an interceptor.  This method is not called directly; instead,
        /// it gets invoked from other public methods.
        /// </summary>
        private ISession GetSession(IInterceptor interceptor)
        {
            if (ContextSession == null)
            {
                if (mSessionFactory == null)
                {
                    InitSessionFactory();
                }
                ContextSession = interceptor == null
                    ? mSessionFactory.OpenSession()
                    : mSessionFactory.OpenSession(interceptor);

                // all retrievals and updates are issued within a transaction
                BeginTransaction(ContextSession);
            }

            return ContextSession;
        }

        /// <summary>
        /// Closes the session.
        /// </summary>
        public void CloseSession()
        {
            if (ContextTransaction != null)
            {
                RollbackTransaction();
            }
            if (ContextSession != null)
            {
                if (ContextSession.IsOpen)
                {
                    ContextSession.Close();
                }
                ContextSession.Dispose();
                ContextSession = null;
            }
        }

        #endregion

        #region Transaction management

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        public void BeginTransaction()
        {
            BeginTransaction(GetSession());
        }

        /// <summary>
        /// Creates a new transaction / Unit of Work for the given session if no transaction is started yet
        /// </summary>
        /// <remarks>
        /// Code Analysis rule can be suppressed because this class is singleton
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private void BeginTransaction(ISession session)
        {
            if (ContextTransaction == null)
            {
                ContextTransaction = session.BeginTransaction();
            }
        }

        /// <summary>
        /// Commits the transaction / Unit of Work if autocommit is set to true, canceltransaction to false and
        /// if the transaction is not committed or rolled back yet
        /// </summary>
        public void CommitTransaction()
        {
            if (AutoCommitTransaction && ContextTransaction != null && !ContextTransaction.WasCommitted
                && !ContextTransaction.WasRolledBack && !CancelTransaction)
            {
                ContextTransaction.Commit();
                ContextTransaction.Dispose();
                ContextTransaction = null;
            }
            CancelTransaction = false;
            AutoCommitTransaction = false;
        }

        /// <summary>
        /// Rollbacks the transaction.
        /// </summary>
        public void RollbackTransaction()
        {
            if (ContextTransaction != null)
            {
                if (!ContextTransaction.WasCommitted && !ContextTransaction.WasRolledBack
                   && !CancelTransaction)
                {
                    ContextTransaction.Rollback();
                }

                ContextTransaction = null;
            }
            CancelTransaction = false;
            AutoCommitTransaction = false;
        }

        /// <summary>
        /// Gets or sets the thread transaction.
        /// </summary>
        /// <value>The thread transaction.</value>
        static internal ITransaction ContextTransaction
        {
            get
            {
                if (IsInWebContext())
                {
                    return (ITransaction)HttpContext.Current.Items[TransactionKey];
                }
                return (ITransaction)CallContext.GetData(TransactionKey);
            }
            set
            {
                if (IsInWebContext())
                {
                    HttpContext.Current.Items[TransactionKey] = value;
                }
                else
                {
                    CallContext.SetData(TransactionKey, value);
                }
            }
        }

        #endregion

        private static bool IsInWebContext()
        {
            return HttpContext.Current != null;
        }

        /// <summary>
        /// Imports the database schema.
        /// </summary>
        public virtual void ImportDatabaseSchema()
        {
            new SchemaExport(mConfiguration).Execute(false, false, false);
        }
        /// <summary>
        /// Exports the database schema.
        /// </summary>
        public virtual void ExportDatabaseSchema()
        {
            new SchemaExport(mConfiguration).Execute(false, true, false);
        }
    }
}
