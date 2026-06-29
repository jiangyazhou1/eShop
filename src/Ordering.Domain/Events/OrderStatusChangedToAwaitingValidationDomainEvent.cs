namespace eShop.Ordering.Domain.Events;

/// <summary>
/// 订单状态变更为等待验证领域事件，在宽限期订单确认时触发
/// </summary>
public class OrderStatusChangedToAwaitingValidationDomainEvent
        : INotification
{
    /// <summary>
    /// 获取订单 ID
    /// </summary>
    public int OrderId { get; }

    /// <summary>
    /// 获取订单项集合
    /// </summary>
    public IEnumerable<OrderItem> OrderItems { get; }

    /// <summary>
    /// 创建订单状态变更为等待验证领域事件实例
    /// </summary>
    /// <param name="orderId">订单 ID</param>
    /// <param name="orderItems">订单项集合</param>
    public OrderStatusChangedToAwaitingValidationDomainEvent(int orderId,
        IEnumerable<OrderItem> orderItems)
    {
        OrderId = orderId;
        OrderItems = orderItems;
    }
}
