using eShop.Basket.API.Repositories;
using eShop.Basket.API.IntegrationEvents.EventHandling.Events;

namespace eShop.Basket.API.IntegrationEvents.EventHandling;

/// <summary>
/// 订单启动事件处理器
/// 当订单创建后删除用户购物车
/// </summary>
/// <param name="repository">购物车仓库</param>
/// <param name="logger">日志记录器</param>
public class OrderStartedIntegrationEventHandler(
    IBasketRepository repository,
    ILogger<OrderStartedIntegrationEventHandler> logger) : IIntegrationEventHandler<OrderStartedIntegrationEvent>
{
    /// <summary>
    /// 处理订单启动事件，删除用户购物车
    /// </summary>
    /// <param name="event">订单启动事件</param>
    public async Task Handle(OrderStartedIntegrationEvent @event)
    {
        logger.LogInformation("处理集成事件：{IntegrationEventId} - ({@IntegrationEvent})", @event.Id, @event);

        // 订单创建后清除购物车数据
        await repository.DeleteBasketAsync(@event.UserId);
    }
}
