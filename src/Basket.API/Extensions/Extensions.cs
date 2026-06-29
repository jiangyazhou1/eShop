using System.Text.Json.Serialization;
using eShop.Basket.API.Repositories;
using eShop.Basket.API.IntegrationEvents.EventHandling;
using eShop.Basket.API.IntegrationEvents.EventHandling.Events;

namespace eShop.Basket.API.Extensions;

/// <summary>
/// Basket.API 服务注册扩展类
/// </summary>
public static class Extensions
{
    /// <summary>
    /// 注册 Basket.API 所需的所有应用程序服务
    /// </summary>
    /// <param name="builder">宿主应用程序构建器</param>
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        // 添加默认身份认证配置
        builder.AddDefaultAuthentication();

        // 添加 Redis 缓存连接
        builder.AddRedisClient("redis");

        // 注册购物车仓库实现
        builder.Services.AddSingleton<IBasketRepository, RedisBasketRepository>();

        // 添加 RabbitMQ 事件总线并注册订单启动事件订阅
        builder.AddRabbitMqEventBus("eventbus")
               .AddSubscription<OrderStartedIntegrationEvent, OrderStartedIntegrationEventHandler>()
               .ConfigureJsonOptions(options => options.TypeInfoResolverChain.Add(IntegrationEventContext.Default));
    }
}

/// <summary>
/// 集成事件序列化上下文（AOT 兼容）
/// </summary>
[JsonSerializable(typeof(OrderStartedIntegrationEvent))]
partial class IntegrationEventContext : JsonSerializerContext
{

}
