using System;
namespace FCT.EPS.WSP.DataAccess
{
    internal interface IGenericRepository<TEntity>
     where TEntity : class
    {
        void Delete(params object[] id);
        void Delete(TEntity entityToDelete);
        System.Collections.Generic.IEnumerable<TEntity> Get(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter = null, Func<System.Linq.IQueryable<TEntity>, System.Linq.IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "", int top = 0);
        TEntity GetByID(params object[] id);
        void Insert(TEntity entity);
        void Update(TEntity entityToUpdate);
    }
}
