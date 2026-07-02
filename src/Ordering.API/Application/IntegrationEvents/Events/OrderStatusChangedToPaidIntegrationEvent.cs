namespace eShop.Ordering.API.Application.IntegrationEvents.Events;

/// <summary>
/// 订单状态变更为已支付集成事件
/// 当订单状态变更为已支付时发布，用于通知其他服务订单支付成功
/// </summary>
public record OrderStatusChangedToPaidIntegrationEvent : IntegrationEvent
{
    /// <summary>获取订单ID</summary>
    public int OrderId { get; }
    /// <summary>获取订单状态</summary>
    public OrderStatus OrderStatus { get; }
    /// <summary>获取购买者名称</summary>
    public string BuyerName { get; }
    /// <summary>获取购买者身份标识</summary>
    public string BuyerIdentityGuid { get; }
    /// <summary>获取订单库存商品列表</summary>
    public IEnumerable<OrderStockItem> OrderStockItems { get; }

    /// <summary>
    /// 初始化 OrderStatusChangedToPaidIntegrationEvent 类的新实例
    /// </summary>
    /// <param name="orderId">订单ID</param>
    /// <param name="orderStatus">订单状态</param>
    /// <param name="buyerName">购买者名称</param>
    /// <param name="buyerIdentityGuid">购买者身份标识</param>
    /// <param name="orderStockItems">订单库存商品列表</param>
    public OrderStatusChangedToPaidIntegrationEvent(int orderId,
        OrderStatus orderStatus, string buyerName, string buyerIdentityGuid,
        IEnumerable<OrderStockItem> orderStockItems)
    {
        OrderId = orderId;
        OrderStockItems = orderStockItems;
        OrderStatus = orderStatus;
        BuyerName = buyerName;
        BuyerIdentityGuid = buyerIdentityGuid;
    }
}

