using System.Linq.Expressions;
using System.Reflection.Metadata;
using OnlineOrderSystem.Commands;
using OnlineOrderSystem.Domain;
using OnlineOrderSystem.EventBus;
using OnlineOrderSystem.Events;

namespace OnlineOrderSystem.ReadModel
{
    public class InMemoryReadModelRepository : IOrderReadModelRepository
    {
        private readonly List<OrderReadModel> _orders = [];
        public InMemoryReadModelRepository(IEventBus eventBus)
        {
            eventBus.Subscribe<OrderPlacedEvent>(HandleOrderPlacedEvent);
            eventBus.Subscribe<OrderCancelledEvent>(HandleOrderCancelledEvent);
            eventBus.Subscribe<OrderUpdatedEvent>(HandleOrderUpdatedEvent);
        }

        private async Task HandleOrderUpdatedEvent(OrderUpdatedEvent @event)
        {
            var readModel = _orders.FirstOrDefault(o => o.Id == @event.AggregateId);
            if (readModel != null)
            {
                readModel.UpdatedAt = @event.OccurredOn;
                readModel.Version = @event.Version;
                readModel.TotalAmount = @event.TotalAmount;

                // Update items
                readModel.Items = [.. @event.Items.Select(item => new OrderItemReadModel
                    {
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        Price = item.Price,
                        Quantity = item.Quantity,
                        Subtotal = item.Subtotal
                    })];

                await UpdateAsync(readModel);
            }
            else
            {
                throw new KeyNotFoundException($"Order with ID {@event.AggregateId} not found.");
            }
        }

        private async Task HandleOrderCancelledEvent(OrderCancelledEvent @event)
        {
            await DeleteAsync(@event.AggregateId);
        }

        private async Task HandleOrderPlacedEvent(OrderPlacedEvent @event)
        {
            var readModel = new OrderReadModel
            {
                Id = @event.AggregateId,
                CustomerId = @event.CustomerId,
                ShippingAddress = @event.ShippingAddress,
                Status = OrderStatus.Pending,
                CreatedAt = @event.OccurredOn,
                Items = [.. @event.Items.Select(item => new OrderItemReadModel
                    {
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        Price = item.Price,
                        Quantity = item.Quantity,
                        Subtotal = item.Subtotal
                    })],
                Version = @event.Version,
                TotalAmount = @event.TotalAmount,
                UpdatedAt = null
            };
            await SaveAsync(readModel);
        }



        public Task DeleteAsync(Guid id)
        {
            var order = _orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                throw new KeyNotFoundException($"Order with ID {id} not found.");
            }

            _orders.Remove(order);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<OrderReadModel>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<OrderReadModel>>(_orders);
        }

        public Task<IEnumerable<OrderReadModel>> GetByConditionAsync(Expression<Func<OrderReadModel, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            var compiledPredicate = predicate.Compile();
            var result = _orders.Where(compiledPredicate).ToList();
            return Task.FromResult<IEnumerable<OrderReadModel>>(result);
        }

        public Task<OrderReadModel?> GetByIdAsync(Guid id)
        {
            var order = _orders.FirstOrDefault(o => o.Id == id);
            return Task.FromResult(order);
        }

        public Task<IEnumerable<OrderReadModel>> GetByStatusAsync(OrderStatus status)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(OrderReadModel entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _orders.Add(entity);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(OrderReadModel entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var existingOrder = _orders.FirstOrDefault(o => o.Id == entity.Id);
            if (existingOrder == null)
            {
                throw new KeyNotFoundException($"Order with ID {entity.Id} not found.");
            }

            existingOrder.CustomerId = entity.CustomerId;
            existingOrder.ShippingAddress = entity.ShippingAddress;
            existingOrder.Status = entity.Status;
            existingOrder.CreatedAt = entity.CreatedAt;
            existingOrder.Items = [.. entity.Items];
            existingOrder.Version = entity.Version;
            existingOrder.TotalAmount = entity.TotalAmount;
            existingOrder.UpdatedAt = entity.UpdatedAt;

            return Task.CompletedTask;
        }
    }
}
