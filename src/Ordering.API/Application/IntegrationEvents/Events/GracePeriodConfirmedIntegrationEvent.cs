namespace eShop.Ordering.API.Application.IntegrationEvents.Events;

/// <summary>
/// 宽限期确认集成事件
/// 当订单的宽限期结束时发布，表示订单不会被取消，将继续进行验证流程
/// </summary>
public record GracePeriodConfirmedIntegrationEvent : IntegrationEvent
{
    /// <summary>获取订单ID</summary>
    public int OrderId { get; }

    /// <summary>
    /// 初始化 GracePeriodConfirmedIntegrationEvent 类的新实例
    /// </summary>
    /// <param name="orderId">订单ID</param>
    public GracePeriodConfirmedIntegrationEvent(int orderId) =>
        OrderId = orderId;
}

