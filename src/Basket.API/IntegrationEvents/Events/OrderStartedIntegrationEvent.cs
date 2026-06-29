namespace eShop.Basket.API.IntegrationEvents.EventHandling.Events;

/// <summary>
/// 订单已启动集成事件
/// 事件命名规范：事件表示"已发生的过去事情"，名称使用过去时
/// 集成事件：可能对其他微服务、限界上下文或外部系统产生副作用的事件
/// </summary>
/// <param name="UserId">用户标识</param>
public record OrderStartedIntegrationEvent(string UserId) : IntegrationEvent;
