namespace eShop.Ordering.API.Application.IntegrationEvents.Events;

/// <summary>
/// 订单状态变更为已提交集成事件
/// </summary>
public record OrderStatusChangedToSubmittedIntegrationEvent : IntegrationEvent
{
    /// <summary>获取订单ID</summary>
    public int OrderId { get; }
    /// <summary>获取订单状态</summary>
    public OrderStatus OrderStatus { get; }
    /// <summary>获取购买者名称</summary>
    public string BuyerName { get; }
    /// <summary>获取购买者身份标识</summary>
    public string BuyerIdentityGuid { get; }

    /// <summary>
    /// 初始化 OrderStatusChangedToSubmittedIntegrationEvent 类的新实例
    /// </summary>
    public OrderStatusChangedToSubmittedIntegrationEvent(
        int orderId, OrderStatus orderStatus, string buyerName, string buyerIdentityGuid)
    {
        OrderId = orderId;
        OrderStatus = orderStatus;
        BuyerName = buyerName;
        BuyerIdentityGuid = buyerIdentityGuid;
    }
}
