namespace eShop.EventBus.Abstractions;

/// <summary>
/// 事件总线接口，定义发布集成事件的契约
/// </summary>
public interface IEventBus
{
    /// <summary>
    /// 发布集成事件到消息总线
    /// </summary>
    /// <param name="event">待发布的事件</param>
    /// <returns>表示异步操作的任务</returns>
    Task PublishAsync(IntegrationEvent @event);
}
