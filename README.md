# Online Order System - Event Sourcing Architecture

## Tổng quan
Hệ thống quản lý đơn hàng demo với kiến trúc **Event Sourcing + CQRS** sử dụng ASP.NET Core 8.0.

**Đặc trưng chính:**
- **Event Sourcing**: Lưu trữ events thay vì state
- **CQRS**: Tách biệt Command (Write) và Query (Read)
- **Event Store**: Audit trail và traceability đầy đủ
- **Optimistic Concurrency**: Version control cho events
- **Real-time Sync**: Event Bus đồng bộ Read Models

## Chức năng chính

### **Quản lý đơn hàng**
- Tạo đơn hàng mới với nhiều sản phẩm
- Cập nhật items trong đơn hàng
- Hủy đơn hàng với lý do
- Xem chi tiết đơn hàng và danh sách tất cả orders

### **Event Sourcing & Traceability**
- Xem lịch sử events của từng đơn hàng
- Audit trail đầy đủ với timestamp và version
- Thống kê Event Store (tổng events, orders, event types)
- State reconstruction từ events

### **CQRS Performance**
- **Write Side**: Event Sourcing với business logic validation
- **Read Side**: Pre-computed Read Models cho query nhanh
- **Event Bus**: Auto-sync giữa Write và Read side

## 📡 API Endpoints

### **Command Side (Write Operations)**
```http
POST   /api/orders          # Tạo đơn hàng mới
PUT    /api/orders/{id}      # Cập nhật items đơn hàng
DELETE /api/orders/{id}      # Hủy đơn hàng
```

### **Query Side (Read Operations)**
```http
GET    /api/orders           # Lấy danh sách tất cả đơn hàng
GET    /api/orders/{id}      # Lấy chi tiết đơn hàng theo ID
```

### **Debug & Event Store (Development)**
```http
GET    /api/debug/events           # Xem tất cả events trong store
GET    /api/debug/events/{orderId} # Xem events của 1 order cụ thể
GET    /api/debug/store-stats      # Thống kê Event Store
```

## Tech Stack
- **Framework**: ASP.NET Core 8.0
- **Architecture**: Event Sourcing + CQRS + Event-Driven Architecture
- **Storage**: In-Memory (Event Store + Read Models)
- **API**: RESTful với Swagger UI
- **Patterns**: Aggregate Root, Domain Events, Optimistic Concurrency

## Command Line Setup
```bash
git clone <repository-url>
cd EventSourcingArchitecture
dotnet restore
dotnet build
dotnet run
```


