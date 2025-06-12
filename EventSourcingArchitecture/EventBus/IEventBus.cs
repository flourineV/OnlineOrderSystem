using OnlineOrderSystem.Events;

namespace OnlineOrderSystem.EventBus
{
    public interface IEventBus
    {
        Task PublishAsync<T>(T @event) where T : BaseEvent;
        Task PublishAsync(IEnumerable<BaseEvent> events);
        // Đăng ký lắng nghe sự kiện theo kiểu rõ ràng (type-safe)
        void Subscribe<T>(Func<T, Task> handler) where T : BaseEvent;
        // Đăng ký lắng nghe event theo kiểu động (runtime), dùng khi không biết sẵn loại sự kiện
        void Subscribe(Type eventType, Func<BaseEvent, Task> handler);
    }
}
