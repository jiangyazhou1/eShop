namespace eShop.Catalog.API.IntegrationEvents;

/// <summary>
/// 目录集成事件服务实现类，负责集成事件的保存和发布
/// 使用 Outbox 模式确保事件的可靠投递
/// </summary>
public sealed class CatalogIntegrationEventService(ILogger<CatalogIntegrationEventService> logger,
    IEventBus eventBus,
    CatalogContext catalogContext,
    IIntegrationEventLogService integrationEventLogService)
    : ICatalogIntegrationEventService, IDisposable
{
    private volatile bool disposedValue;

    /// <summary>
    /// 通过事件总线发布集成事件，并更新事件状态
    /// </summary>
    /// <param name="evt">要发布的集成事件</param>
    public async Task PublishThroughEventBusAsync(IntegrationEvent evt)
    {
        try
        {
            logger.LogInformation("Publishing integration event: {IntegrationEventId_published} - ({@IntegrationEvent})", evt.Id, evt);

            await integrationEventLogService.MarkEventAsInProgressAsync(evt.Id);
            await eventBus.PublishAsync(evt);
            await integrationEventLogService.MarkEventAsPublishedAsync(evt.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error Publishing integration event: {IntegrationEventId} - ({@IntegrationEvent})", evt.Id, evt);
            await integrationEventLogService.MarkEventAsFailedAsync(evt.Id);
        }
    }

    /// <summary>
    /// 保存集成事件及目录上下文变更，使用弹性事务确保原子性
    /// </summary>
    /// <param name="evt">要保存的集成事件</param>
    public async Task SaveEventAndCatalogContextChangesAsync(IntegrationEvent evt)
    {
        logger.LogInformation("CatalogIntegrationEventService - Saving changes and integrationEvent: {IntegrationEventId}", evt.Id);

        // 在单个 BeginTransaction 中使用多个 DbContext 时，需要 EF Core 弹性策略
        // 参考：https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency
        await ResilientTransaction.New(catalogContext).ExecuteAsync(async () =>
        {
            // 通过本地事务，实现原始目录数据库操作与 IntegrationEventLog 之间的原子性
            await catalogContext.SaveChangesAsync();
            await integrationEventLogService.SaveEventAsync(evt, catalogContext.Database.CurrentTransaction);
        });
    }

    private void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                (integrationEventLogService as IDisposable)?.Dispose();
            }

            disposedValue = true;
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
