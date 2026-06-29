using System.ComponentModel.DataAnnotations;

namespace eShop.IntegrationEventLogEF;

/// <summary>
/// 集成事件日志条目实体，记录事件的发布状态及内容
/// </summary>
public class IntegrationEventLogEntry
{
    private static readonly JsonSerializerOptions s_indentedOptions = new() { WriteIndented = true };
    private static readonly JsonSerializerOptions s_caseInsensitiveOptions = new() { PropertyNameCaseInsensitive = true };

    /// <summary>
    /// 私有默认构造函数，供 EF Core 使用
    /// </summary>
    private IntegrationEventLogEntry() { }

    /// <summary>
    /// 创建集成事件日志条目实例
    /// </summary>
    /// <param name="event">集成事件</param>
    /// <param name="transactionId">数据库事务标识符</param>
    public IntegrationEventLogEntry(IntegrationEvent @event, Guid transactionId)
    {
        EventId = @event.Id;
        CreationTime = @event.CreationDate;
        EventTypeName = @event.GetType().FullName;
        Content = JsonSerializer.Serialize(@event, @event.GetType(), s_indentedOptions);
        State = EventStateEnum.NotPublished;
        TimesSent = 0;
        TransactionId = transactionId;
    }

    /// <summary>
    /// 获取事件的唯一标识符
    /// </summary>
    public Guid EventId { get; private set; }

    /// <summary>
    /// 获取事件的完整类型名称（必填）
    /// </summary>
    [Required]
    public string EventTypeName { get; private set; }

    /// <summary>
    /// 获取事件的简短类型名称（不含命名空间）
    /// </summary>
    [NotMapped]
    public string EventTypeShortName => EventTypeName.Split('.')?.Last();

    /// <summary>
    /// 获取或设置反序列化后的集成事件对象
    /// </summary>
    [NotMapped]
    public IntegrationEvent IntegrationEvent { get; private set; }

    /// <summary>
    /// 获取或设置事件的当前发布状态
    /// </summary>
    public EventStateEnum State { get; set; }

    /// <summary>
    /// 获取或设置事件发送尝试次数
    /// </summary>
    public int TimesSent { get; set; }

    /// <summary>
    /// 获取条目的创建时间
    /// </summary>
    public DateTime CreationTime { get; private set; }

    /// <summary>
    /// 获取或设置事件的 JSON 序列化内容（必填）
    /// </summary>
    [Required]
    public string Content { get; private set; }

    /// <summary>
    /// 获取关联的数据库事务标识符
    /// </summary>
    public Guid TransactionId { get; private set; }

    /// <summary>
    /// 将 JSON 内容反序列化为指定类型的集成事件
    /// </summary>
    /// <param name="type">目标事件类型</param>
    /// <returns>设置 IntegrationEvent 属性后的当前实例</returns>
    public IntegrationEventLogEntry DeserializeJsonContent(Type type)
    {
        IntegrationEvent = JsonSerializer.Deserialize(Content, type, s_caseInsensitiveOptions) as IntegrationEvent;
        return this;
    }
}
