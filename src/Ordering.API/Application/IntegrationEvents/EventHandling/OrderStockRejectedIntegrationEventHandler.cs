namespace eShop.Ordering.API.Application.IntegrationEvents.EventHandling;

/// <summary>
/// 订单库存拒绝集成事件处理器
/// 当收到库存拒绝事件时，发送命令更新订单状态为库存验证失败
/// </summary>
public class OrderStockRejectedIntegrationEventHandler(
    IMediator mediator,
    ILogger<OrderStockRejectedIntegrationEventHandler> logger) : IIntegrationEventHandler<OrderStockRejectedIntegrationEvent>
{
    /// <summary>
    /// 处理订单库存拒绝集成事件
    /// 提取库存不足的商品ID列表，发送命令设置订单状态为库存验证失败
    /// </summary>
    public async Task Handle(OrderStockRejectedIntegrationEvent @event)
    {
        logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", @event.Id, @event);

        // 提取库存不足的商品ID列表
        var orderStockRejectedItems = @event.OrderStockItems
            .FindAll(c => !c.HasStock)
            .Select(c => c.ProductId)
            .ToList();

        var command = new SetStockRejectedOrderStatusCommand(@event.OrderId, orderStockRejectedItems);

        logger.LogInformation(
            "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
            command.GetGenericTypeName(),
            nameof(command.OrderNumber),
            command.OrderNumber,
            command);

        await mediator.Send(command);
    }
}
