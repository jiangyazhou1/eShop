namespace eShop.Ordering.API.Application.IntegrationEvents.Events;

/// <summary>
/// 订单库存拒绝集成事件
/// 当库存服务拒绝订单库存请求时发布
/// </summary>
public record OrderStockRejectedIntegrationEvent : IntegrationEvent
{
    /// <summary>获取订单ID</summary>
    public int OrderId { get; }

    /// <summary>获取确认的库存商品列表</summary>
    public List<ConfirmedOrderStockItem> OrderStockItems { get; }

    /// <summary>
    /// 初始化 OrderStockRejectedIntegrationEvent 类的新实例
    /// </summary>
    public OrderStockRejectedIntegrationEvent(int orderId,
        List<ConfirmedOrderStockItem> orderStockItems)
    {
        OrderId = orderId;
        OrderStockItems = orderStockItems;
    }
}

/// <summary>
/// 已确认的订单库存商品记录
/// </summary>
public record ConfirmedOrderStockItem
{
    /// <summary>获取商品ID</summary>
    public int ProductId { get; }
    /// <summary>获取是否有库存</summary>
    public bool HasStock { get; }

    /// <summary>
    /// 初始化 ConfirmedOrderStockItem 类的新实例
    /// </summary>
    public ConfirmedOrderStockItem(int productId, bool hasStock)
    {
        ProductId = productId;
        HasStock = hasStock;
    }
}
