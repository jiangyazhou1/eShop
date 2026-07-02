namespace eShop.Ordering.API.Application.IntegrationEvents.Events;

/// <summary>
/// 订单状态变更为已取消集成事件
/// 当订单状态变更为已取消时发布，用于通知其他服务订单已被取消
/// </summary>
public record OrderStatusChangedToCancelledIntegrationEvent : IntegrationEvent
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
    /// 初始化 OrderStatusChangedToCancelledIntegrationEvent 类的新实例
    /// </summary>
    /// <param name="orderId">订单ID</param>
    /// <param name="orderStatus">订单状态</param>
    /// <param name="buyerName">购买者名称</param>
    /// <param name="buyerIdentityGuid">购买者身份标识</param>
    public OrderStatusChangedToCancelledIntegrationEvent
        (int orderId, OrderStatus orderStatus, string buyerName, string buyerIdentityGuid)
    {
        OrderId = orderId;
        OrderStatus = orderStatus;
        BuyerName = buyerName;
        BuyerIdentityGuid = buyerIdentityGuid;
    }
}
