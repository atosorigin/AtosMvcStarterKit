using Customer.Project.DataAccess.RepositoryInterfaces;

namespace Customer.Project.DataAccess.NHibernateHelpers
{
    public class TransactionManager : ITransactionManager
    {
        #region ITransactionManager Members

        public void BeginTransaction()
        {
            NHibernateSessionManager.Instance.BeginTransaction();
        }
        public void RollbackTransaction()
        {
            NHibernateSessionManager.Instance.RollbackTransaction();
        }
        public void CommitTransaction()
        {
            NHibernateSessionManager.Instance.CommitTransaction();
        }
        #endregion
    }
}
