namespace eShop.Ordering.API.Application.IntegrationEvents;

/// <summary>
/// 订单集成事件服务接口，定义集成事件的保存和发布操作
/// </summary>
public interface IOrderingIntegrationEventService
{
    /// <summary>
    /// 通过事件总线发布指定事务的所有待发布集成事件
    /// </summary>
    Task PublishEventsThroughEventBusAsync(Guid transactionId);

    /// <summary>
    /// 添加并保存集成事件到事件日志
    /// </summary>
    Task AddAndSaveEventAsync(IntegrationEvent evt);
}
