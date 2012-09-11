namespace Customer.Project.DataAccess.RepositoryInterfaces.Int
{
    public interface IBaseRepositoryInt<T>
    {
        void Delete(T entity);

        T SaveOrUpdate(T entity);
    }
}
