using AilosContaCorrente.Domain.Entities;
using System.Linq.Expressions;

namespace AilosContaCorrente.Domain
{
    public interface IRepository<T> where T : EntityBase
    {
        Task<List<T>> GetAll();
        Task<T> GetFirst(object id);
        Task<T> GetFirst(Expression<Func<T, bool>> predicate);
        Task<List<T>> GetAll(Expression<Func<T, bool>> predicate);
        Task<T> Insert(T entity);
        Task<List<T>> Insert(List<T> entities);
        Task<T> Update(T entity);
        Task<List<T>> Update(List<T> entities);
        Task<T> Disable(T entity);
        Task<List<T>> Disable(List<T> entities);
        Task Delete(object id);
        Task DeleteRange(object[] ids);
    }
}