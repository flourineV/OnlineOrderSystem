namespace OnlineOrderSystem.Domain
{
    public enum OrderStatus
    {
        Pending = 0,    // Đơn hàng mới, có thể update
        Cancelled = 1   // Đã hủy, không thể thay đổi
    }
}
