# Online Order System - Event Sourcing Architecture

## 📋 Tổng quan dự án
Hệ thống quản lý đơn hàng trực tuyến sử dụng kiến trúc Event Sourcing với ASP.NET Core 8.0. Dự án áp dụng các pattern CQRS (Command Query Responsibility Segregation), Event Sourcing, và Domain-Driven Design.

## 🏗️ Kiến trúc hệ thống
```
├── Commands/           # Command handlers và commands
├── Controllers/        # API Controllers
├── Domain/            # Domain models và business logic
├── DTO/               # Data Transfer Objects
├── EventBus/          # Event publishing và handling
├── Events/            # Domain events
├── EventStore/        # Event storage implementation
├── Infrastructure/    # Dependency injection và configuration
├── Queries/           # Query handlers
├── ReadModel/         # Read model repositories
└── Properties/        # Application properties
```

## 👥 Phân chia công việc cho Team (2 người)

### 🧑‍💻 **Developer 1 - Command Side (Write Model)**

#### **Files cần code:**
- [ ] `Domain/Order.cs`
- [ ] `Domain/OrderItem.cs`
- [ ] `Domain/OrderStatus.cs`
- [ ] `Events/BaseEvent.cs`
- [ ] `Events/OrderPlacedEvent.cs`
- [ ] `Events/OrderCancelledEvent.cs` *(cần tạo mới)*
- [ ] `Events/OrderUpdatedEvent.cs` *(cần tạo mới)*
- [ ] `EventStore/IEventStore.cs`
- [ ] `EventStore/InMemoryEventStore.cs`
- [ ] `EventStore/EventModel.cs`
- [ ] `Commands/PlaceOrderCommand.cs`
- [ ] `Commands/PlaceOrderCommandHandler.cs`
- [ ] `Commands/CancelOrderCommand.cs`
- [ ] `Commands/CancelOrderCommandHandler.cs`
- [ ] `Commands/UpdateOrderCommand.cs` *(cần tạo mới)*
- [ ] `Commands/UpdateOrderCommandHandler.cs` *(cần tạo mới)*

---

### 🧑‍💻 **Developer 2 - Query Side (Read Model) & API**

#### **Files cần code:**
- [ ] `Controllers/OrderController.cs` *(cần tạo mới)*
- [ ] `DTO/PlaceOrderDto.cs`
- [ ] `DTO/OrderDto.cs`
- [ ] `DTO/OrderItemDto.cs` *(cần tạo mới)*
- [ ] `DTO/UpdateOrderDto.cs` *(cần tạo mới)*
- [ ] `Queries/GetOrderQuery.cs` *(cần tạo mới)*
- [ ] `Queries/GetOrdersQuery.cs` *(cần tạo mới)*
- [ ] `Queries/OrderQueryHandler.cs` *(cần tạo mới)*
- [ ] `ReadModel/OrderReadModel.cs`
- [ ] `ReadModel/IReadModelRepository.cs` *(cần tạo mới)*
- [ ] `ReadModel/InMemoryReadModelRepository.cs`
- [ ] `EventBus/IEventBus.cs`
- [ ] `EventBus/InMemoryEventBus.cs`
- [ ] `EventBus/EventHandlerDelegates.cs`

---

## 🤝 **Shared Files (Cần phối hợp)**

### **Files cả 2 người cần làm việc cùng:**
- [ ] `Infrastructure/DependencyInjection.cs` - DI configuration
- [ ] `Program.cs` - Application startup configuration
- [ ] `appsettings.json` / `appsettings.Development.json` - Configuration

### **Integration Points cần sync:**
- **Interfaces**: Đảm bảo IEventStore, IEventBus, IReadModelRepository contracts đúng
- **DTOs**: Command DTOs phải match với Domain models
- **Events**: Event structure phải consistent giữa Command và Query side
- **Error Handling**: Unified error response format

## 🛠️ Tech Stack
- **Framework**: ASP.NET Core 8.0
- **Architecture**: Event Sourcing + CQRS
- **Storage**: In-Memory (có thể mở rộng sang SQL Server/PostgreSQL)
- **API**: RESTful API với Swagger
- **Testing**: xUnit (sẽ thêm sau)

## 🚀 Getting Started

### Prerequisites
- .NET 8.0 SDK
- Visual Studio 2022 hoặc VS Code
- Git

### Setup
```bash
git clone <repository-url>
cd EventSourcingArchitecture
dotnet restore
dotnet build
dotnet run
```

### API Endpoints (Sẽ implement)
```
POST /api/orders          # Place new order
GET  /api/orders/{id}     # Get order by ID
GET  /api/orders          # Get all orders
PUT  /api/orders/{id}     # Update order
DELETE /api/orders/{id}   # Cancel order
```

## 📝 Notes
- Tuân thủ SOLID principles
- Implement proper error handling
- Add logging cho tất cả operations
- Code review trước khi merge
- Unit tests cho business logic
- Integration tests cho API endpoints

## 📋 **Task Summary**

### **Developer 1 (Command Side)**: 16 files
- Domain models (3 files)
- Events (4 files)
- Event Store (3 files)
- Commands (6 files)

### **Developer 2 (Query Side & API)**: 14 files
- Controllers & DTOs (5 files)
- Queries (3 files)
- Read Models (3 files)
- Event Bus (3 files)

### **Shared**: 3 files
- Infrastructure setup
- Application configuration

---

## 🤝 **Collaboration Notes**
- **Code review**: Bắt buộc cho shared files
- **Interface contracts**: Sync trước khi implement
- **Branch naming**: `feature/dev1-command/...` hoặc `feature/dev2-query/...`
- **Dependencies**: Developer 2 cần interfaces từ Developer 1 trước

---
**Team**: 2 Developers | **Total**: 33 files