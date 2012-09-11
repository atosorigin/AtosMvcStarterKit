using System;
using System.Linq;
using Customer.Project.DataAccess.NHibernateHelpers;
using Customer.Project.DataAccess.RepositoryInterfaces;
using Customer.Project.DataAccess.RepositoryInterfaces.Int;
using Customer.Project.Domain.Entities;
using NHibernate;
using NHibernate.Linq;

namespace Customer.Project.DataAccess.Repositories
{
    public class BaseRepository<T> : IBaseRepositoryInt<T>, IBaseRepository<T> where T : class, IEntityId
    {
        protected virtual ISession Session
        {
            get
            {
                return NHibernateSessionManager.Instance.GetSession();
            }
        }
        public virtual T Get(Guid id)
        {
            return id == Guid.Empty ? default(T) : Session.Get<T>(id);
        }
        public virtual T Get(long id)
        {
            return id <= 0 ? default(T) : Session.Get<T>(id);
        }
        /// <summary>
        /// Throws InvalidOperationException if no item is found with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T GetSingle(Guid id)
        {
            T t = Session.Get<T>(id);
            if (t == null)
                throw new InvalidOperationException("Sequence contains no elements");
            return t;
        }
        /// <summary>
        /// Throws InvalidOperationException if no item is found with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T GetSingle(long id)
        {
            T t = Session.Get<T>(id);
            if (t == null)
                throw new InvalidOperationException("Sequence contains no elements");
            return t;
        }

        /// <summary>
        /// Get List of Entitities.
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<T> GetAll()
        {
            return Session.Query<T>();
        }

        /// <summary>
        /// Insert Or Update Pet into the datasource.
        /// </summary>
        /// <param name="pet"></param>
        /// <returns>
        /// The inserted Or update Pet with any additional values come from the datasource.
        /// </returns>
        public virtual T SaveOrUpdate(T entity)
        {
            bool insert = entity.IdOfEntity <= 0;

            if (insert)
                Insert(entity);
            else
                Update(entity);

            return entity;
        }

        /// <summary>
        /// Insert Entity into the datasource.
        /// </summary>
        /// <param name="pet"></param>
        /// <returns>
        /// The inserted Entity with any additional values come from the datasource.
        /// </returns>
        public virtual T Insert(T entity)
        {
            Session.Save(entity);

            // have to set the committransaction flag to commit to database at the closure of the Unit of Work
            NHibernateSessionManager.Instance.AutoCommitTransaction = true;

            return entity;
        }

        /// <summary>
        /// Update entity in the datasource.
        /// </summary>
        /// <param name="pet"></param>
        /// <returns>
        /// The updated entity with any additional values come from the datasource.
        /// </returns>
        public virtual T Update(T entity)
        {
            Session.SaveOrUpdate(entity);

            // have to set the committransaction flag to commit to database
            NHibernateSessionManager.Instance.AutoCommitTransaction = true;

            return entity;
        }

        /// <summary>
        /// Delete Entity from the datasource.
        /// </summary>
        /// <param name="pet"></param>
        public virtual void Delete(T entity)
        {
            Session.Delete(entity);

            // have to set the committransaction flag to commit to database at the closure of the Unit of Work
            NHibernateSessionManager.Instance.AutoCommitTransaction = true;
        }

        /// <summary>
        /// Delete Entity from the datasource.
        /// </summary>
        /// <param name="pet"></param>
        public virtual void Delete(Guid id)
        {
            Session.Delete(Session.Get<T>(id));

            // have to set the committransaction flag to commit to database at the closure of the Unit of Work
            NHibernateSessionManager.Instance.AutoCommitTransaction = true;
        }
    }
}
