namespace eShop.Ordering.API.Application.IntegrationEvents;

/// <summary>
/// 订单集成事件服务实现类，负责集成事件的保存和发布
/// </summary>
public class OrderingIntegrationEventService(IEventBus eventBus,
    OrderingContext orderingContext,
    IIntegrationEventLogService integrationEventLogService,
    ILogger<OrderingIntegrationEventService> logger) : IOrderingIntegrationEventService
{
    /// <summary>获取事件总线实例</summary>
    private readonly IEventBus _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    /// <summary>获取订单数据库上下文实例</summary>
    private readonly OrderingContext _orderingContext = orderingContext ?? throw new ArgumentNullException(nameof(orderingContext));
    /// <summary>获取集成事件日志服务实例</summary>
    private readonly IIntegrationEventLogService _eventLogService = integrationEventLogService ?? throw new ArgumentNullException(nameof(integrationEventLogService));
    /// <summary>获取日志记录器实例</summary>
    private readonly ILogger<OrderingIntegrationEventService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    /// 通过事件总线发布指定事务的所有待发布集成事件
    /// </summary>
    /// <param name="transactionId">事务ID</param>
    public async Task PublishEventsThroughEventBusAsync(Guid transactionId)
    {
        // 获取待发布的事件日志
        var pendingLogEvents = await _eventLogService.RetrieveEventLogsPendingToPublishAsync(transactionId);

        foreach (var logEvt in pendingLogEvents)
        {
            _logger.LogInformation("Publishing integration event: {IntegrationEventId} - ({@IntegrationEvent})", logEvt.EventId, logEvt.IntegrationEvent);

            try
            {
                // 标记事件为处理中
                await _eventLogService.MarkEventAsInProgressAsync(logEvt.EventId);
                // 通过事件总线发布事件
                await _eventBus.PublishAsync(logEvt.IntegrationEvent);
                // 标记事件为已发布
                await _eventLogService.MarkEventAsPublishedAsync(logEvt.EventId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error publishing integration event: {IntegrationEventId}", logEvt.EventId);

                // 标记事件为发布失败
                await _eventLogService.MarkEventAsFailedAsync(logEvt.EventId);
            }
        }
    }

    /// <summary>
    /// 添加并保存集成事件到事件日志存储
    /// </summary>
    /// <param name="evt">集成事件</param>
    public async Task AddAndSaveEventAsync(IntegrationEvent evt)
    {
        _logger.LogInformation("Enqueuing integration event {IntegrationEventId} to repository ({@IntegrationEvent})", evt.Id, evt);

        // 保存事件到数据库事务
        await _eventLogService.SaveEventAsync(evt, _orderingContext.GetCurrentTransaction());
    }
}
