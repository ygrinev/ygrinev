using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FCT.LLC.GenericRepository.Contracts
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Find(params object[] keyValues);
        IQueryable<TEntity> SqlQuery(string query, params object[] parameters);
        void ExecuteQuery(string query, params object[] parameters);
        void Insert(TEntity entity);
        void InsertRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entities);
        void Delete(TEntity entity);
        IQueryFluent<TEntity> Query(IQueryObject<TEntity> queryObject);
        IQueryFluent<TEntity> Query(Expression<Func<TEntity, bool>> query);
        IQueryFluent<TEntity> Query();
        IQueryable<TEntity> GetAll();
    }
}