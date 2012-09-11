namespace Customer.Project.DataAccess.RepositoryInterfaces
{
    public interface ITransactionManager
    {
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
    }
}
