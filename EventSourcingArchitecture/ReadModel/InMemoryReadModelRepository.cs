using System.Linq.Expressions;
using OnlineOrderSystem.Domain;

namespace OnlineOrderSystem.ReadModel
{
    public class InMemoryReadModelRepository : IOrderReadModelRepository
    {
        public Task<OrderReadModel> GetByIdAsync(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrderReadModel>> GetByStatusAsync(OrderStatus status)
        {
            throw new NotImplementedException();
        }
    }
}
