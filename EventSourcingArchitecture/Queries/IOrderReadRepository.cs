using OnlineOrderSystem.Domain;
using OnlineOrderSystem.ReadModel;

namespace OnlineOrderSystem.Queries
{
    public interface IOrderReadRepository
    {
        Task<OrderReadModel> GetByIdAsync(Guid orderId);
        Task<IEnumerable<OrderReadModel>> GetByStatusAsync(OrderStatus status);
        Task<IEnumerable<OrderReadModel>> GetAllAsync();
    }
}
