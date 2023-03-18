using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DIMARCore.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task Create(T entidad);
        Task<T> GetById(object id);
        Task Update(T entidad);
        IQueryable<T> GetAllAsQueryable();
        Task<T> GetWithCondition(Expression<Func<T, bool>> whereCondition);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAllWithCondition(Expression<Func<T, bool>> whereCondition);
        Task<IEnumerable<T>> GetAllWithConditionAsync(Expression<Func<T, bool>> whereCondition);
        Task<bool> AnyWithCondition(Expression<Func<T, bool>> whereCondition);
    }
}
