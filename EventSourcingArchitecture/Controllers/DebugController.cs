using Microsoft.AspNetCore.Mvc;
using OnlineOrderSystem.EventStore;
using OnlineOrderSystem.Events;
using OnlineOrderSystem.ReadModel;

namespace OnlineOrderSystem.Controllers
{
    [ApiController]
    [Route("api/debug")]
    public class DebugController : ControllerBase
    {
        private readonly IEventStore _eventStore;
        private readonly ILogger<DebugController> _logger;

        public DebugController(IEventStore eventStore, ILogger<DebugController> logger)
        {
            _eventStore = eventStore;
            _logger = logger;
        }

        /// <summary>
        /// Xem tất cả events trong Event Store
        /// </summary>
        [HttpGet("events")]
        public async Task<ActionResult<object>> GetAllEvents()
        {
            try
            {
                _logger.LogInformation("Retrieving all events from Event Store");
                
                var events = await _eventStore.GetAllEventsAsync();
                
                var result = new
                {
                    TotalEvents = events.Count(),
                    Events = events.Select(e => new
                    {
                        EventId = e.Id,
                        EventType = e.EventType,
                        AggregateId = e.AggregateId,
                        Version = e.Version,
                        OccurredOn = e.OccurredOn,
                        EventData = e // Full event object
                    }).OrderBy(e => e.OccurredOn)
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all events");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Xem events của một Order cụ thể
        /// </summary>
        [HttpGet("events/{orderId}")]
        public async Task<ActionResult<object>> GetEventsByOrderId(Guid orderId)
        {
            try
            {
                _logger.LogInformation("Retrieving events for Order {OrderId}", orderId);
                
                var events = await _eventStore.GetEventsAsync(orderId);
                
                if (!events.Any())
                {
                    return NotFound($"No events found for Order {orderId}");
                }

                var result = new
                {
                    OrderId = orderId,
                    EventCount = events.Count(),
                    Events = events.Select(e => new
                    {
                        EventId = e.Id,
                        EventType = e.EventType,
                        Version = e.Version,
                        OccurredOn = e.OccurredOn,
                        EventData = e // Full event object
                    }).OrderBy(e => e.Version)
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving events for Order {OrderId}", orderId);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Thống kê Event Store
        /// </summary>
        [HttpGet("store-stats")]
        public async Task<ActionResult<object>> GetEventStoreStats()
        {
            try
            {
                _logger.LogInformation("Retrieving Event Store statistics");
                
                var allEvents = await _eventStore.GetAllEventsAsync();
                
                var stats = new
                {
                    TotalEvents = allEvents.Count(),
                    TotalOrders = allEvents.Select(e => e.AggregateId).Distinct().Count(),
                    EventTypes = allEvents.GroupBy(e => e.EventType)
                        .Select(g => new { EventType = g.Key, Count = g.Count() })
                        .OrderByDescending(x => x.Count),
                    OrdersWithEventCounts = allEvents.GroupBy(e => e.AggregateId)
                        .Select(g => new { 
                            OrderId = g.Key, 
                            EventCount = g.Count(),
                            LastEventTime = g.Max(e => e.OccurredOn)
                        })
                        .OrderByDescending(x => x.LastEventTime),
                    TimeRange = allEvents.Any() ? new
                    {
                        FirstEvent = allEvents.Min(e => e.OccurredOn),
                        LastEvent = allEvents.Max(e => e.OccurredOn)
                    } : null
                };

                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Event Store statistics");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
