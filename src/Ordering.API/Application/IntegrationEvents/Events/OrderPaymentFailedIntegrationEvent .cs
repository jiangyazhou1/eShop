namespace eShop.Ordering.API.Application.IntegrationEvents.Events;

/// <summary>
/// 订单支付失败集成事件
/// 当订单支付失败时发布，将触发订单取消流程
/// </summary>
public record OrderPaymentFailedIntegrationEvent : IntegrationEvent
{
    /// <summary>获取订单ID</summary>
    public int OrderId { get; }

    /// <summary>
    /// 初始化 OrderPaymentFailedIntegrationEvent 类的新实例
    /// </summary>
    /// <param name="orderId">订单ID</param>
    public OrderPaymentFailedIntegrationEvent(int orderId) => OrderId = orderId;
}
