namespace eShop.Ordering.API.Application.IntegrationEvents.EventHandling;

/// <summary>
/// 订单库存确认集成事件处理器
/// 当收到库存确认事件时，发送命令更新订单状态为库存已确认
/// </summary>
public class OrderStockConfirmedIntegrationEventHandler(
    IMediator mediator,
    ILogger<OrderStockConfirmedIntegrationEventHandler> logger) :
    IIntegrationEventHandler<OrderStockConfirmedIntegrationEvent>
{
    /// <summary>
    /// 处理订单库存确认集成事件
    /// 发送命令设置订单状态为库存已确认
    /// </summary>
    public async Task Handle(OrderStockConfirmedIntegrationEvent @event)
    {
        logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", @event.Id, @event);

        var command = new SetStockConfirmedOrderStatusCommand(@event.OrderId);

        logger.LogInformation(
            "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
            command.GetGenericTypeName(),
            nameof(command.OrderNumber),
            command.OrderNumber,
            command);

        await mediator.Send(command);
    }
}
