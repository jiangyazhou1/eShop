namespace eShop.Ordering.Domain.Events;

/// <summary>
/// 订单状态变更为库存已确认领域事件，在订单商品库存确认时触发
/// </summary>
public class OrderStatusChangedToStockConfirmedDomainEvent
    : INotification
{
    /// <summary>
    /// 获取订单 ID
    /// </summary>
    public int OrderId { get; }

    /// <summary>
    /// 创建订单状态变更为库存已确认领域事件实例
    /// </summary>
    /// <param name="orderId">订单 ID</param>
    public OrderStatusChangedToStockConfirmedDomainEvent(int orderId)
        => OrderId = orderId;
}
