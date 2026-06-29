using eShop.EventBusRabbitMQ;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Hosting;

/// <summary>
/// RabbitMQ 依赖注入扩展类，将 RabbitMQ 事件总线注册到服务容器
/// </summary>
public static class RabbitMqDependencyInjectionExtensions
{
    // 配置节格式示例：
    // {
    //   "EventBus": {
    //     "SubscriptionClientName": "...",
    //     "RetryCount": 10
    //   }
    // }

    /// <summary>
    /// EventBus 配置节的名称
    /// </summary>
    private const string SectionName = "EventBus";

    /// <summary>
    /// 将 RabbitMQ 事件总线添加到应用宿主构建器
    /// </summary>
    /// <param name="builder">宿主应用构建器</param>
    /// <param name="connectionName">连接字符串名称</param>
    /// <returns>事件总线构建器，用于进一步配置</returns>
    public static IEventBusBuilder AddRabbitMqEventBus(this IHostApplicationBuilder builder, string connectionName)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.AddRabbitMQClient(connectionName);

        // RabbitMQ.Client 不支持内置的 OpenTelemetry，需手动添加
        builder.Services.AddOpenTelemetry()
           .WithTracing(tracing =>
           {
               tracing.AddSource(RabbitMQTelemetry.ActivitySourceName);
           });

        // 配置选项支持，从 EventBus 配置节读取
        builder.Services.Configure<EventBusOptions>(builder.Configuration.GetSection(SectionName));

        // 封装核心客户端 API 的抽象层
        builder.Services.AddSingleton<RabbitMQTelemetry>();
        builder.Services.AddSingleton<IEventBus, RabbitMQEventBus>();
        // 应用启动后立即开始消费消息
        builder.Services.AddSingleton<IHostedService>(sp => (RabbitMQEventBus)sp.GetRequiredService<IEventBus>());

        return new EventBusBuilder(builder.Services);
    }

    private class EventBusBuilder(IServiceCollection services) : IEventBusBuilder
    {
        public IServiceCollection Services => services;
    }
}
