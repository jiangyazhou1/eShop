namespace eShop.EventBus.Abstractions;

/// <summary>
/// 泛型集成事件处理器接口，处理指定类型的集成事件
/// </summary>
/// <typeparam name="TIntegrationEvent">事件类型</typeparam>
public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
    where TIntegrationEvent : IntegrationEvent
{
    /// <summary>
    /// 处理指定类型的集成事件
    /// </summary>
    /// <param name="event">待处理的事件</param>
    /// <returns>表示异步操作的任务</returns>
    Task Handle(TIntegrationEvent @event);

    /// <summary>
    /// 显式实现：将事件转换为泛型类型后处理
    /// </summary>
    /// <param name="event">待处理的事件</param>
    /// <returns>表示异步操作的任务</returns>
    Task IIntegrationEventHandler.Handle(IntegrationEvent @event) => Handle((TIntegrationEvent)@event);
}

/// <summary>
/// 集成事件处理器基接口，定义处理事件的契约
/// </summary>
public interface IIntegrationEventHandler
{
    /// <summary>
    /// 处理集成事件
    /// </summary>
    /// <param name="event">待处理的事件</param>
    /// <returns>表示异步操作的任务</returns>
    Task Handle(IntegrationEvent @event);
}
