using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using eShop.EventBus.Abstractions;
using eShop.EventBus.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// 事件总线构建器扩展方法，用于配置 JSON 选项和注册事件订阅
/// </summary>
public static class EventBusBuilderExtensions
{
    /// <summary>
    /// 配置事件总线的 JSON 序列化选项
    /// </summary>
    /// <param name="eventBusBuilder">事件总线构建器实例</param>
    /// <param name="configure">JSON 序列化选项配置回调</param>
    /// <returns>配置后的事件总线构建器</returns>
    public static IEventBusBuilder ConfigureJsonOptions(this IEventBusBuilder eventBusBuilder, Action<JsonSerializerOptions> configure)
    {
        eventBusBuilder.Services.Configure<EventBusSubscriptionInfo>(o =>
        {
            configure(o.JsonSerializerOptions);
        });

        return eventBusBuilder;
    }

    /// <summary>
    /// 注册事件订阅，将指定事件类型与处理器绑定
    /// </summary>
    /// <typeparam name="T">集成事件类型</typeparam>
    /// <typeparam name="TH">事件处理器类型</typeparam>
    /// <param name="eventBusBuilder">事件总线构建器实例</param>
    /// <returns>配置后的事件总线构建器</returns>
    public static IEventBusBuilder AddSubscription<T, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TH>(this IEventBusBuilder eventBusBuilder)
        where T : IntegrationEvent
        where TH : class, IIntegrationEventHandler<T>
    {
        // 使用 Keyed Services 注册同一事件类型的多个处理器
        // 消费者可通过 IKeyedServiceProvider.GetKeyedService<IIntegrationEventHandler>(typeof(T)) 获取所有处理器
        eventBusBuilder.Services.AddKeyedTransient<IIntegrationEventHandler, TH>(typeof(T));

        eventBusBuilder.Services.Configure<EventBusSubscriptionInfo>(o =>
        {
            // 记录所有注册的事件类型及其名称映射，消息总线中传递事件时避免使用 Type.GetType
            // 此列表也将用于订阅底层消息中间件中的事件
            o.EventTypes[typeof(T).Name] = typeof(T);
        });

        return eventBusBuilder;
    }
}
