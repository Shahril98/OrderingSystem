using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using OrderingSystem.Infrastructure.Databases.OrderingSystem;
using OrderingSystem.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using OrderingSystem.Application.Utils;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace OrderingSystem.Application.Repositories
{
    public interface IBaseRepository
    {
        void AddData<T>(T entity) where T : class;
        void AddDataBatch<T>(IEnumerable<T> entities) where T : class;
        Task CommitTransaction();
        void DeleteData<T>(T entity) where T : class;
        void DeleteDataBatch<T>(IEnumerable<T> entities) where T : class;
        Task DeleteDataById<T>(Guid guid) where T : class;
        Task DeleteDataById_E<T>(Guid id, Guid? userId = null) where T : TblBaseSoftDelete;
        Task<List<T>> GetAllData<T>(bool isTracked = true) where T : class;
        Task<List<T>> GetAllDataWithCondition<T>(Expression<Func<T, bool>> predicate, bool isTracked = true) where T : class;
        Task<T> GetDataById<T>(Guid id) where T : class;
        Task<bool> IsExistAsync<T>(Expression<Func<T, bool>> predicate) where T : class;
        Task RollbackTransaction();
        Task<int> SaveChanges(Guid? userId = null);
        Task<IDbContextTransaction> StartTransaction();
        void UpdateData<T>(T entity) where T : class;
        void UpdateDataBatch<T>(IEnumerable<T> entities) where T : class;
    }

    public class BaseRepository(OrderingSystemDbContext context) : IBaseRepository
    {
        private IDbContextTransaction _transaction;
        /// <summary>
        /// Dont forget to call commitTransaction After start to apply changes!!!
        /// Also dont forget to get the return value and use using!!!!
        /// </summary>
        /// <returns></returns>
        public async Task<IDbContextTransaction> StartTransaction()
        {
            _transaction = await context.Database.BeginTransactionAsync();
            return _transaction;
        }
        public async Task CommitTransaction()
        {
            await _transaction.CommitAsync();
        }
        public async Task RollbackTransaction()
        {
            await _transaction.RollbackAsync();
        }
        #region read
        public async Task<List<T>> GetAllData<T>(bool isTracked = true) where T : class
        {
            if (isTracked)
            {
                return await context.Set<T>().ToListAsync();
            }
            else
            {
                return await context.Set<T>().AsNoTracking().ToListAsync();
            }
        }
        public async Task<T> GetDataById<T>(Guid id) where T : class
        {
            var result = await context.Set<T>().FindAsync(id);
            return result is null ? throw new RecordNotFoundException($"Tiada rekod dijumpai untuk id = '{id}'") : result;
        }
        public async Task<List<T>> GetAllDataWithCondition<T>(Expression<Func<T, bool>> predicate, bool isTracked = true) where T : class
        {
            var query = GetTrackedOrNot<T>(isTracked);
            return await query.Where(predicate).ToListAsync();
        }

        private IQueryable<T> GetTrackedOrNot<T>(bool isTracked = true) where T : class
        {
            return isTracked ? context.Set<T>() : context.Set<T>().AsNoTracking();
        }
        #endregion
        #region insert/update/delete
        public void AddData<T>(T entity) where T : class
        {
            context.Set<T>().Add(entity);
        }
        public void AddDataBatch<T>(IEnumerable<T> entities) where T : class
        {
            context.Set<T>().AddRange(entities);
        }
        public void UpdateData<T>(T entity) where T : class
        {
            context.Set<T>().Update(entity);
        }
        public void UpdateDataBatch<T>(IEnumerable<T> entities) where T : class
        {
            context.Set<T>().UpdateRange(entities);
        }
        public void DeleteData<T>(T entity) where T : class
        {
            context.Set<T>().Remove(entity);
        }
        public async Task<bool> IsExistAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            var count = await context.Set<T>().CountAsync(predicate);
            return count != 0;
        }
        public async Task DeleteDataById<T>(Guid guid) where T : class
        {
            var toRemove = await context.Set<T>().FindAsync(guid) ?? throw new RecordNotFoundException("Tiada rekod ditemui untuk dipadam");
            context.Set<T>().Remove(toRemove);
        }
        /// <summary>
        /// this method will not have audit trail inserted
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="RecordNotFoundException"></exception>
        public async Task DeleteDataById_E<T>(Guid id, Guid? userId = null) where T : TblBaseSoftDelete
        {
            //soft delete using executeUpdate
            var r = await context.Set<T>().Where(x => x.Id == id)
                .ExecuteUpdateAsyncCustom(setter => setter, userId, true);
            if (r == 0)
                throw new RecordNotFoundException($"Tiada rekod dengan id '{id}' untuk dipadam");
        }
        public void DeleteDataBatch<T>(IEnumerable<T> entities) where T : class
        {
            context.Set<T>().RemoveRange(entities);
        }
        #endregion
        public async Task<int> SaveChanges(Guid? userId = null)
        {
            var entries = Enumerable.ToList(context.ChangeTracker.Entries());
            foreach (var entry in entries)
            {
                if (entry.Entity != null && entry.Entity.GetType().IsSubclassOf(typeof(TblBaseSoftDelete)))
                {
                    var entity = entry.Entity as TblBaseSoftDelete;
                    if (entity != null)
                    {
                        if (entry.State == EntityState.Deleted)
                        {
                            entry.State = EntityState.Modified;
                            entity.IsDeleted = true;
                            entity.TimestampDeleted = DateTimeOffset.UtcNow;
                            entity.TimestampUpdated = DateTimeOffset.UtcNow;
                        }
                        else if (entry.State == EntityState.Added)
                        {
                            entity.Id = Guid.NewGuid();
                            entity.TimestampUpdated = DateTimeOffset.UtcNow;
                            entity.TimestampCreated = DateTimeOffset.UtcNow;
                        }
                        else if (entry.State == EntityState.Modified)
                        {
                            entity.TimestampUpdated = DateTimeOffset.UtcNow;
                        }
                    }
                }
                else if (entry.Entity != null && entry.Entity.GetType().IsSubclassOf(typeof(TblBase)))
                {
                    var entity = entry.Entity as TblBase;
                    if (entity is not null)
                    {
                        if (entry.State == EntityState.Added)
                        {
                            entity.Id = Guid.NewGuid();
                            entity.TimestampUpdated = DateTimeOffset.UtcNow;
                            entity.TimestampCreated = DateTimeOffset.UtcNow;
                        }
                        else if (entry.State == EntityState.Modified)
                        {
                            entity.TimestampUpdated = DateTimeOffset.UtcNow;
                        }
                    }
                }
                    if (userId is not null && entry.Entity is not null)
                {
                    var auditTrail = new TblAuditTrail
                    {
                        Id = Guid.NewGuid(),
                        Action = entry.State,
                        ActionTimestamp = DateTimeOffset.UtcNow,
                        ActorId = userId.Value,
                        TableName = entry.Entity.GetType().Name,
                        Data = JsonSerializer.Serialize(entry.Entity, new JsonSerializerOptions { WriteIndented = false })
                    };
                    if (entry.Entity.GetType().IsSubclassOf(typeof(TblBaseSoftDelete))){
                        var entity = entry.Entity as TblBaseSoftDelete;
                        if (entity is not null)
                            auditTrail.TableId = entity.Id;
                    }
                    else if (entry.Entity.GetType().IsSubclassOf(typeof(TblBase))){
                        var entity = entry.Entity as TblBase;
                        if (entity is not null)
                            auditTrail.TableId = entity.Id;
                    }
                    context.TblAuditTrail.Add(auditTrail);
                }
            }
            return await context.SaveChangesAsync();
        }
    }
}
