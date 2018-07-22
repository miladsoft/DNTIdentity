#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using ASPNETCoreIdentitySample.DataLayer.Context;
using LinqKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

#endregion

namespace ASPNETCoreIdentitySample.Services
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        #region Private Fields

        private readonly DbSet<TEntity> _dbSet;
        private readonly IUnitOfWork _unitOfWork;

        #endregion Private Fields

        public Repository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dbSet = unitOfWork.Set<TEntity>();
        }

        public  void Delete(object id)
        {
            var entity = _dbSet.Find(id);
            _unitOfWork.Entry(entity).State = EntityState.Deleted;

        }

        public void Delete(TEntity entity)
        {
            _unitOfWork.Entry(entity).State = EntityState.Deleted;


        }

        public async Task<bool> DeleteAsync(params object[] keyValues)
        {
            var entity = await FindAsync(keyValues);

            if (entity == null)
            {
                return false;
            }

            _unitOfWork.Entry(entity).State = EntityState.Deleted;
            return true;
        }

        public async Task<bool> DeleteAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            var entity = await FindAsync(cancellationToken, keyValues);

            if (entity == null)
            {
                return false;
            }

            _unitOfWork.Entry(entity).State = EntityState.Deleted;
            return true;
        }

        public virtual TEntity Find(params object[] keyValues)
        {
            return _dbSet.Find(keyValues);
        }

        public async Task<TEntity> FindAsync(params object[] keyValues)
        {
            return await _dbSet.FindAsync(keyValues);
        }

        public async Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            return await _dbSet.FindAsync(cancellationToken, keyValues);
        }

        public void Insert(TEntity entity)
        {
            _unitOfWork.Entry(entity).State = EntityState.Added;
        }

        public void InsertGraphRange(IEnumerable<TEntity> entities)
        {
            foreach (var item in entities)
            {
                Insert(item);
            }
        }

        public void InsertOrUpdateGraph(TEntity entity)
        {
            _dbSet.AddRange(entity);
        }

        public void InsertRange(IEnumerable<TEntity> entities)
        {
            foreach (var item in entities)
            {
                Insert(item);
            }
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

        public IQueryable<TEntity> Queryable()
        {
            return _dbSet;
        }

        public IQueryable<TEntity> SelectQuery(string query, params object[] parameters)
        {
            return _dbSet.FromSql(query, parameters).AsQueryable();
        }

        public void Update(TEntity entity)
        {
            _unitOfWork.Entry(entity).State = EntityState.Modified;

        }


        internal IQueryable<TEntity> Select(
    Expression<Func<TEntity, bool>> filter = null,
    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
    List<Expression<Func<TEntity, object>>> includes = null,
    int? page = null,
    int? pageSize = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (page != null && pageSize != null)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }
            return query;
        }

        internal async Task<IEnumerable<TEntity>> SelectAsync(
    Expression<Func<TEntity, bool>> filter = null,
    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
    List<Expression<Func<TEntity, object>>> includes = null,
    int? page = null,
    int? pageSize = null)
        {
            return await Select(filter, orderBy, includes, page, pageSize).ToListAsync();
        }
    }
}