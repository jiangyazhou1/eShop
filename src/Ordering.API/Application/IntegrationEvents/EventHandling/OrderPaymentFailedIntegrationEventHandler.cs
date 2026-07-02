namespace eShop.Ordering.API.Application.IntegrationEvents.EventHandling;

/// <summary>
/// 订单支付失败集成事件处理器
/// </summary>
public class OrderPaymentFailedIntegrationEventHandler(
    IMediator mediator,
    ILogger<OrderPaymentFailedIntegrationEventHandler> logger) :
    IIntegrationEventHandler<OrderPaymentFailedIntegrationEvent>
{
    /// <summary>
    /// 处理订单支付失败集成事件
    /// 取消订单
    /// </summary>
    /// <param name="event">支付失败事件</param>
    public async Task Handle(OrderPaymentFailedIntegrationEvent @event)
    {
        logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", @event.Id, @event);

        // 发送取消订单命令
        var command = new CancelOrderCommand(@event.OrderId);

        logger.LogInformation(
            "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
            command.GetGenericTypeName(),
            nameof(command.OrderNumber),
            command.OrderNumber,
            command);

        await mediator.Send(command);
    }
}
