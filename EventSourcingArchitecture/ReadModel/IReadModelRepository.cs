using OnlineOrderSystem.Domain;
using System.Linq.Expressions;

namespace OnlineOrderSystem.ReadModel
{
    public interface IReadModelRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetByConditionAsync(Expression<Func<T, bool>> predicate);
        Task SaveAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
    }

    public interface IOrderReadModelRepository : IReadModelRepository<OrderReadModel>
    {
        Task<IEnumerable<OrderReadModel>> GetByStatusAsync(OrderStatus status);
    }
}
