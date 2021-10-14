using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LMSG3.Core.Repositories
{
    public interface IRepository<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter,
                                                            Func<IQueryable<TEntity>,
                                                            IOrderedQueryable<TEntity>> orderBy = null);
        Task<TEntity> GetAsync(int? id);
        Task<TEntity> FindAsync(int? id);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Remove(int id);
        void Remove(TEntity entity);
    }
}
