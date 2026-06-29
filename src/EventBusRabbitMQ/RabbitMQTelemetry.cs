using System.Diagnostics;
using OpenTelemetry.Context.Propagation;

namespace eShop.EventBusRabbitMQ;

/// <summary>
/// RabbitMQ 遥测类，提供 ActivitySource 和 Propagator 用于分布式链路追踪
/// </summary>
public class RabbitMQTelemetry
{
    /// <summary>
    /// 获取 ActivitySource 的名称，用于创建追踪活动
    /// </summary>
    public static string ActivitySourceName = "EventBusRabbitMQ";

    /// <summary>
    /// 获取 ActivitySource 实例，用于创建 RabbitMQ 相关的 Activity 活动
    /// </summary>
    public ActivitySource ActivitySource { get; } = new(ActivitySourceName);

    /// <summary>
    /// 获取文本映射传播器，用于上下文传播
    /// </summary>
    public TextMapPropagator Propagator { get; } = Propagators.DefaultTextMapPropagator;
}
