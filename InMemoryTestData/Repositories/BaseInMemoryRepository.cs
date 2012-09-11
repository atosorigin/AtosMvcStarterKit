using System;
using System.Collections.Generic;
using System.Linq;
using Customer.Project.DataAccess.RepositoryInterfaces;
using Customer.Project.DataAccess.RepositoryInterfaces.Int;
using Customer.Project.Domain.Entities;

namespace InMemoryTestData.Repositories
{
    /// <summary>
    /// Repository that maintains an inmemory collection of the designated Entity. Supports all default CRUD operations
    /// on the entity. Use the AddTestData method to add test data to the Items collection.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseInMemoryRepository<T> : IBaseRepositoryInt<T>, IBaseRepository<T> where T : class, IEntityId
    {
        protected static IList<T> Items { get; set; }

        public BaseInMemoryRepository()
        {
            if (Items == null)
            {
                Items = new List<T>();
            }
            if (Items.Count == 0)
            {
                AddTestData();
            }
        }

        #region Sample data

        protected static void AddTestData()
        {
            if (typeof(T) == typeof(Product ))
            {
                Items.Add(new Product
                              {
                    IdOfEntity = 1,
                    Name = "Test1",
                    RawPrice = 10
                } as T);

                Items.Add(new Product
                {
                    IdOfEntity = 2,
                    Name = "Test2",
                    RawPrice = 20
                } as T);
            }
        }

        #endregion

        #region IBaseRepository<T> Members

        public virtual void Initialize(object obj)
        {

        }

        public virtual T Get(long id)
        {
            return Items.FirstOrDefault(item => item.IdOfEntity == id);
        }

        public virtual IQueryable<T> GetAll()
        {
            return Items.AsQueryable<T>();
        }

        public virtual T SaveOrUpdate(T entity)
        {
            if (entity.IdOfEntity == 0)
            {
                Items.Add(entity);
                entity.IdOfEntity = GetNewIdOfEntity();
            }
            else
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    if (Items[i].IdOfEntity == entity.IdOfEntity)
                        Items[i] = entity;
                }
            }
            return entity;
        }

        protected virtual long GetNewIdOfEntity()
        {
            long id = -1;
            foreach (var item in Items)
            {
                if (item.IdOfEntity > id)
                {
                    id = item.IdOfEntity;
                }
            }
            return (id + 1);
        }

        public void Delete(T entity)
        {
            Items.Remove(entity);
        }

        #endregion

        #region IBaseRepository<T> Members

        public T Get(System.Guid id)
        {
            throw new NotImplementedException();
        }

        public T GetSingle(System.Guid id)
        {
            throw new NotImplementedException();
        }

        public T GetSingle(long id)
        {
            return Items.Single(item => item.IdOfEntity == id);
        }

        #endregion
    }
}
