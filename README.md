# Online Order System - Event Sourcing Architecture

## Tổng quan
Hệ thống quản lý đơn hàng với kiến trúc **Event Sourcing + CQRS** sử dụng ASP.NET Core 8.0.

## Cấu trúc Project
```
EventSourcingArchitecture/
├── Controllers/        # API Controllers (Write + Read)
├── Commands/          # Command handlers
├── Domain/           # Business logic & Aggregates
├── DTO/              # Data Transfer Objects
├── Events/           # Domain Events
├── EventStore/       # Event Storage
├── EventBus/         # Event Publishing
├── Queries/          # Query handlers (TODO)
└── ReadModel/        # Read Models (TODO)
```

## **COMMAND SIDE**

### **API Layer**
- `Controllers/OrderWriteController.cs` - 3 endpoints (POST, PUT, DELETE)

### **DTO Layer**
- `DTO/PlaceOrderRequest.cs` + `OrderItemRequest`
- `DTO/UpdateOrderRequest.cs`
- `DTO/CancelOrderRequest.cs`
- `DTO/OrderResponse.cs`
- `DTO/ApiErrorResponse.cs`

### **Command Layer**
- `Commands/PlaceOrderCommand.cs`
- `Commands/UpdateOrderCommand.cs`
- `Commands/CancelOrderCommand.cs`
- `Commands/PlaceOrderCommandHandler.cs`
- `Commands/UpdateOrderCommandHandler.cs`
- `Commands/CancelOrderCommandHandler.cs`

### **Domain Layer**
- `Domain/Order.cs` - Aggregate Root với Event Sourcing
- `Domain/OrderItem.cs` - Value Object
- `Domain/OrderStatus.cs` - Enum

### **Events Layer**
- `Events/BaseEvent.cs`
- `Events/OrderPlacedEvent.cs`
- `Events/OrderUpdatedEvent.cs`
- `Events/OrderCancelledEvent.cs`

### **Infrastructure Layer**
- `EventStore/IEventStore.cs` + `InMemoryEventStore.cs`
- `EventBus/IEventBus.cs` + `InMemoryEventBus.cs`
- `EventStore/EventModel.cs`

## **QUERY SIDE**

## **API Endpoints**

### **Command Side (Write)**
```http
POST   /api/orders          # Tạo đơn hàng mới
PUT    /api/orders/{id}      # Cập nhật đơn hàng
DELETE /api/orders/{id}      # Hủy đơn hàng
```

### **Query Side (Read)**
```http
GET    /api/orders/{id}      # Lấy đơn hàng theo ID
GET    /api/orders           # Lấy danh sách đơn hàng
```

## **Tech Stack**
- **Framework**: ASP.NET Core 8.0
- **Architecture**: Event Sourcing + EDA + CQRS
- **Storage**: In-Memory
- **API**: RESTful với Swagger

## 🚀 **Getting Started**
```bash
git clone <repository-url>
cd EventSourcingArchitecture
dotnet restore
dotnet build
dotnet run
```

