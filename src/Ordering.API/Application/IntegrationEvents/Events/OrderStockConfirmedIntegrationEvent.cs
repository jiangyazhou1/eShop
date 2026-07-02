namespace eShop.Ordering.API.Application.IntegrationEvents.Events;

/// <summary>
/// 订单库存确认集成事件
/// </summary>
public record OrderStockConfirmedIntegrationEvent : IntegrationEvent
{
    /// <summary>获取订单ID</summary>
    public int OrderId { get; }

    /// <summary>
    /// 初始化 OrderStockConfirmedIntegrationEvent 类的新实例
    /// </summary>
    public OrderStockConfirmedIntegrationEvent(int orderId) => OrderId = orderId;
}
