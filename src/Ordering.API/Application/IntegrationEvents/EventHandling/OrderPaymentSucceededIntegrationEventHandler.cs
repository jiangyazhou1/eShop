namespace eShop.Ordering.API.Application.IntegrationEvents.EventHandling;

/// <summary>
/// 订单支付成功集成事件处理器
/// </summary>
public class OrderPaymentSucceededIntegrationEventHandler(
    IMediator mediator,
    ILogger<OrderPaymentSucceededIntegrationEventHandler> logger) :
    IIntegrationEventHandler<OrderPaymentSucceededIntegrationEvent>
{
    /// <summary>
    /// 处理订单支付成功集成事件
    /// 将订单状态更新为已支付
    /// </summary>
    /// <param name="event">支付成功事件</param>
    public async Task Handle(OrderPaymentSucceededIntegrationEvent @event)
    {
        logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", @event.Id, @event);

        // 发送设置订单状态为已支付的命令
        var command = new SetPaidOrderStatusCommand(@event.OrderId);

        logger.LogInformation(
            "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
            command.GetGenericTypeName(),
            nameof(command.OrderNumber),
            command.OrderNumber,
            command);

        await mediator.Send(command);
    }
}
