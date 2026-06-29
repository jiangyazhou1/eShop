namespace eShop.Catalog.API.IntegrationEvents.EventHandling;

/// <summary>
/// 订单状态变更（等待验证）集成事件处理程序
/// 当订单进入等待验证状态时，检查商品库存是否充足，然后发布库存确认或拒绝事件
/// </summary>
public class OrderStatusChangedToAwaitingValidationIntegrationEventHandler(
    CatalogContext catalogContext,
    ICatalogIntegrationEventService catalogIntegrationEventService,
    ILogger<OrderStatusChangedToAwaitingValidationIntegrationEventHandler> logger) :
    IIntegrationEventHandler<OrderStatusChangedToAwaitingValidationIntegrationEvent>
{
    /// <summary>
    /// 处理订单等待验证事件
    /// 遍历订单中的商品项检查库存，库存不足则发布 OrderStockRejectedIntegrationEvent，否则发布 OrderStockConfirmedIntegrationEvent
    /// </summary>
    /// <param name="event">等待验证集成事件</param>
    public async Task Handle(OrderStatusChangedToAwaitingValidationIntegrationEvent @event)
    {
        logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", @event.Id, @event);

        // 存储每个商品的库存确认结果
        var confirmedOrderStockItems = new List<ConfirmedOrderStockItem>();

        // 遍历订单中的商品项，检查库存是否充足
        foreach (var orderStockItem in @event.OrderStockItems)
        {
            var catalogItem = catalogContext.CatalogItems.Find(orderStockItem.ProductId);
            if (catalogItem is not null)
            {
                // 判断商品可用库存是否大于等于订单请求数量
                var hasStock = catalogItem.AvailableStock >= orderStockItem.Units;
                var confirmedOrderStockItem = new ConfirmedOrderStockItem(catalogItem.Id, hasStock);

                confirmedOrderStockItems.Add(confirmedOrderStockItem);
            }
        }

        // 如果有任何商品库存不足，发布库存拒绝事件；否则发布库存确认事件
        var confirmedIntegrationEvent = confirmedOrderStockItems.Any(c => !c.HasStock)
            ? (IntegrationEvent)new OrderStockRejectedIntegrationEvent(@event.OrderId, confirmedOrderStockItems)
            : new OrderStockConfirmedIntegrationEvent(@event.OrderId);

        // 保存事件到数据库并发布到事件总线
        await catalogIntegrationEventService.SaveEventAndCatalogContextChangesAsync(confirmedIntegrationEvent);
        await catalogIntegrationEventService.PublishThroughEventBusAsync(confirmedIntegrationEvent);
    }
}
