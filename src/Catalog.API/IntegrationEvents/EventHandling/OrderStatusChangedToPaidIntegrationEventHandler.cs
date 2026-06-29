namespace eShop.Catalog.API.IntegrationEvents.EventHandling;

/// <summary>
/// 订单状态变更（已支付）集成事件处理程序
/// 当订单支付成功时，扣减商品库存
/// </summary>
public class OrderStatusChangedToPaidIntegrationEventHandler(
    CatalogContext catalogContext,
    ILogger<OrderStatusChangedToPaidIntegrationEventHandler> logger) :
    IIntegrationEventHandler<OrderStatusChangedToPaidIntegrationEvent>
{
    /// <summary>
    /// 处理订单已支付事件
    /// 遍历订单中的商品项，从目录库存中扣减相应数量
    /// </summary>
    /// <param name="event">订单已支付集成事件</param>
    public async Task Handle(OrderStatusChangedToPaidIntegrationEvent @event)
    {
        logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", @event.Id, @event);

        // 遍历订单中的商品项，从库存中扣减对应数量
        foreach (var orderStockItem in @event.OrderStockItems)
        {
            var catalogItem = catalogContext.CatalogItems.Find(orderStockItem.ProductId);

            catalogItem?.RemoveStock(orderStockItem.Units);
        }

        await catalogContext.SaveChangesAsync();
    }
}
