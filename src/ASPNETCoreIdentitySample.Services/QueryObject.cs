using System;
using System.Linq.Expressions;
using LinqKit;

namespace ASPNETCoreIdentitySample.Services
{
    public abstract class QueryObject<TEntity> 
    {
        private Expression<Func<TEntity, bool>> _query;

        public virtual Expression<Func<TEntity, bool>> Query()
        {
            return _query;
        }


    }
}