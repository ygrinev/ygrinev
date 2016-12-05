#region

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using FCT.LLC.GenericRepository.Contracts;
using LinqKit;

#endregion

namespace FCT.LLC.GenericRepository.Implementations
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _dbSet;


        public Repository(DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentException("context");
            }
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public virtual TEntity Find(params object[] keyValues)
        {
            _context.Configuration.AutoDetectChangesEnabled = false;
            return _dbSet.Find(keyValues);
        }

        public virtual IQueryable<TEntity> SqlQuery(string query, params object[] parameters)
        {
            return _dbSet.SqlQuery(query, parameters).AsQueryable();
        }

        public void ExecuteQuery(string query, params object[] parameters)
        {
            _context.Database.ExecuteSqlCommand(query, parameters);
        }

        public virtual void Insert(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Added;
            _dbSet.Add(entity);
        }

        public virtual void InsertRange(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
        }

        public virtual void Update(TEntity entity)
        {
            _context.Entry(entity).State=EntityState.Modified;

        }

        public virtual void DeleteRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }

        public virtual void Delete(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Deleted;
            _dbSet.Remove(entity);
        }

        public IQueryFluent<TEntity> Query()
        {
            return new QueryFluent<TEntity>(this);
        }

        public virtual IQueryFluent<TEntity> Query(IQueryObject<TEntity> queryObject)
        {
            return new QueryFluent<TEntity>(this, queryObject);
        }

        public virtual IQueryFluent<TEntity> Query(Expression<Func<TEntity, bool>> query)
        {
            return new QueryFluent<TEntity>(this, query);
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbSet.AsNoTracking();
        }

        internal IQueryable<TEntity> Select(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null,
            int? page = null,
            int? pageSize = null, bool tracking = true)
        {
            IQueryable<TEntity> query = _dbSet;

            //Set AsNoTracking 
            if(!tracking)
            {
                query = query.AsNoTracking();
            }            

            if (includes != null)
                query = includes.Aggregate(query, (current, include) => current.Include(include));

            if (orderBy != null)
                query = orderBy(query);

            if (filter != null)
                query = query.AsExpandable().Where(filter);

            if (page != null && pageSize != null)
                query = query.Skip((page.Value - 1)*pageSize.Value).Take(pageSize.Value);

            return query;
        }

    }
}