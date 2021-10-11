using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSG3.Core.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        void Add(TEntity entity);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetAsync(int? id);
        Task<TEntity> FindAsync(int? id);
        void Update(TEntity entity);
        void Remove(int id);
        void Remove(TEntity entity);    
    }
}
