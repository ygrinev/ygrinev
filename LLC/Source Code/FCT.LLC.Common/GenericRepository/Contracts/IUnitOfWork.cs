using System;
using System.Data;
using System.Data.Entity;
using FCT.LLC.GenericRepository.Implementations;

namespace FCT.LLC.GenericRepository.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();
        void Dispose(bool disposing);
        IRepository<TEntity> Repository<TEntity>() where TEntity : class;
        DbContextTransaction TransactionScope { get; }
        bool Commit();
        void Rollback();
    }
}