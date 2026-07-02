namespace eShop.Ordering.API.Application.IntegrationEvents.EventHandling;

/// <summary>
/// 宽限期确认集成事件处理器
/// 当宽限期确认事件到达时，表示订单不会在初始阶段被取消，因此订单处理继续进入验证阶段
/// </summary>
public class GracePeriodConfirmedIntegrationEventHandler(
    IMediator mediator,
    ILogger<GracePeriodConfirmedIntegrationEventHandler> logger) : IIntegrationEventHandler<GracePeriodConfirmedIntegrationEvent>
{
    /// <summary>
    /// 处理宽限期确认集成事件
    /// 将订单状态更新为等待验证
    /// </summary>
    /// <param name="event">宽限期确认事件</param>
    public async Task Handle(GracePeriodConfirmedIntegrationEvent @event)
    {
        logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", @event.Id, @event);

        // 发送设置订单状态为等待验证的命令
        var command = new SetAwaitingValidationOrderStatusCommand(@event.OrderId);

        logger.LogInformation(
            "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
            command.GetGenericTypeName(),
            nameof(command.OrderNumber),
            command.OrderNumber,
            command);

        await mediator.Send(command);
    }
}
