namespace eShop.Ordering.API.Application.IntegrationEvents.Events;

/// <summary>
/// 订单支付成功集成事件
/// 当订单支付成功时发布，将触发订单状态更新为已支付
/// </summary>
public record OrderPaymentSucceededIntegrationEvent : IntegrationEvent
{
    /// <summary>获取订单ID</summary>
    public int OrderId { get; }

    /// <summary>
    /// 初始化 OrderPaymentSucceededIntegrationEvent 类的新实例
    /// </summary>
    /// <param name="orderId">订单ID</param>
    public OrderPaymentSucceededIntegrationEvent(int orderId) => OrderId = orderId;
}
