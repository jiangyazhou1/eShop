namespace eShop.EventBus.Events;

/// <summary>
/// 集成事件基类，表示在限界上下文之间传递的事件
/// </summary>
public record IntegrationEvent
{
    /// <summary>
    /// 创建集成事件实例，自动分配唯一标识与创建时间
    /// </summary>
    public IntegrationEvent()
    {
        Id = Guid.NewGuid();
        CreationDate = DateTime.UtcNow;
    }

    /// <summary>
    /// 获取事件的唯一标识符（创建时自动生成）
    /// </summary>
    [JsonInclude]
    public Guid Id { get; set; }

    /// <summary>
    /// 获取事件的创建时间（UTC 时间）
    /// </summary>
    [JsonInclude]
    public DateTime CreationDate { get; set; }
}
