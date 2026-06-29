namespace eShop.IntegrationEventLogEF;

/// <summary>
/// 集成事件状态枚举，表示事件在发布流程中的当前状态
/// </summary>
public enum EventStateEnum
{
    /// <summary>
    /// 尚未发布，事件已保存但未发送至消息总线
    /// </summary>
    NotPublished = 0,

    /// <summary>
    /// 正在发布中，事件正在发送至消息总线
    /// </summary>
    InProgress = 1,

    /// <summary>
    /// 发布成功，事件已成功发送至消息总线
    /// </summary>
    Published = 2,

    /// <summary>
    /// 发布失败，事件发送至消息总线时发生错误
    /// </summary>
    PublishedFailed = 3
}

