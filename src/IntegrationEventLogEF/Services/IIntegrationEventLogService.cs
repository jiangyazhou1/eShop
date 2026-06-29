namespace eShop.IntegrationEventLogEF.Services;

/// <summary>
/// 集成事件日志服务接口，定义事件日志操作的契约
/// </summary>
public interface IIntegrationEventLogService
{
    /// <summary>
    /// 检索指定事务中待发布的事件日志列表
    /// </summary>
    /// <param name="transactionId">事务标识符</param>
    /// <returns>待发布的事件日志集合</returns>
    Task<IEnumerable<IntegrationEventLogEntry>> RetrieveEventLogsPendingToPublishAsync(Guid transactionId);

    /// <summary>
    /// 保存集成事件到日志表
    /// </summary>
    /// <param name="event">集成事件</param>
    /// <param name="transaction">数据库事务</param>
    /// <returns>表示异步保存操作的任务</returns>
    Task SaveEventAsync(IntegrationEvent @event, IDbContextTransaction transaction);

    /// <summary>
    /// 标记事件为已发布状态
    /// </summary>
    /// <param name="eventId">事件标识符</param>
    /// <returns>表示异步操作的任务</returns>
    Task MarkEventAsPublishedAsync(Guid eventId);

    /// <summary>
    /// 标记事件为处理中状态
    /// </summary>
    /// <param name="eventId">事件标识符</param>
    /// <returns>表示异步操作的任务</returns>
    Task MarkEventAsInProgressAsync(Guid eventId);

    /// <summary>
    /// 标记事件为发布失败状态
    /// </summary>
    /// <param name="eventId">事件标识符</param>
    /// <returns>表示异步操作的任务</returns>
    Task MarkEventAsFailedAsync(Guid eventId);
}
