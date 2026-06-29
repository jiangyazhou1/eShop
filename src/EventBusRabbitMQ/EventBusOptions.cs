namespace eShop.EventBusRabbitMQ;

/// <summary>
/// 事件总线配置选项类，配置 RabbitMQ 消息订阅客户端行为
/// </summary>
public class EventBusOptions
{
    /// <summary>
    /// 获取或设置订阅客户端名称，用于标识当前服务实例
    /// </summary>
    public string SubscriptionClientName { get; set; }

    /// <summary>
    /// 获取或设置重试次数，默认为 10 次
    /// </summary>
    public int RetryCount { get; set; } = 10;
}
