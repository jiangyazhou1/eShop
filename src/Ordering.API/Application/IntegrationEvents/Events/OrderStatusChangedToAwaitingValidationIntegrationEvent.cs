namespace eShop.Ordering.API.Application.IntegrationEvents.Events;

/// <summary>
/// 订单状态变更为等待验证集成事件
/// 当订单状态变更为等待验证时发布，用于通知其他服务订单正在等待验证
/// </summary>
public record OrderStatusChangedToAwaitingValidationIntegrationEvent : IntegrationEvent
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
    /// 初始化 OrderStatusChangedToAwaitingValidationIntegrationEvent 类的新实例
    /// </summary>
    /// <param name="orderId">订单ID</param>
    /// <param name="orderStatus">订单状态</param>
    /// <param name="buyerName">购买者名称</param>
    /// <param name="buyerIdentityGuid">购买者身份标识</param>
    /// <param name="orderStockItems">订单库存商品列表</param>
    public OrderStatusChangedToAwaitingValidationIntegrationEvent(
        int orderId, OrderStatus orderStatus, string buyerName, string buyerIdentityGuid,
        IEnumerable<OrderStockItem> orderStockItems)
    {
        OrderId = orderId;
        OrderStockItems = orderStockItems;
        OrderStatus = orderStatus;
        BuyerName = buyerName;
        BuyerIdentityGuid = buyerIdentityGuid;
    }
}

/// <summary>
/// 订单库存商品记录，表示订单中的单个商品库存信息
/// </summary>
public record OrderStockItem
{
    /// <summary>获取商品ID</summary>
    public int ProductId { get; }
    /// <summary>获取商品数量</summary>
    public int Units { get; }

    /// <summary>
    /// 初始化 OrderStockItem 类的新实例
    /// </summary>
    /// <param name="productId">商品ID</param>
    /// <param name="units">商品数量</param>
    public OrderStockItem(int productId, int units)
    {
        ProductId = productId;
        Units = units;
    }
}
