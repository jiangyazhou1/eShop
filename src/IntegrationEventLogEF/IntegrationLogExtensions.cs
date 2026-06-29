namespace eShop.IntegrationEventLogEF;

/// <summary>
/// 集成事件日志扩展方法，配置 EF Core 实体映射
/// </summary>
public static class IntegrationLogExtensions
{
    /// <summary>
    /// 配置集成事件日志的实体映射
    /// </summary>
    /// <param name="builder">模型构建器</param>
    public static void UseIntegrationEventLogs(this ModelBuilder builder)
    {
        builder.Entity<IntegrationEventLogEntry>(builder =>
        {
            // 将 IntegrationEventLogEntry 映射到 IntegrationEventLog 表
            builder.ToTable("IntegrationEventLog");

            // 配置 EventId 为主键
            builder.HasKey(e => e.EventId);
        });
    }
}
