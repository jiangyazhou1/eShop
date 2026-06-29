namespace eShop.Catalog.API.IntegrationEvents;

/// <summary>
/// 目录集成事件服务接口，负责管理集成事件的保存和发布
/// </summary>
public interface ICatalogIntegrationEventService
{
    /// <summary>
    /// 保存集成事件及目录上下文变更（使用本地事务确保原子性）
    /// </summary>
    /// <param name="evt">要保存的集成事件</param>
    Task SaveEventAndCatalogContextChangesAsync(IntegrationEvent evt);

    /// <summary>
    /// 通过事件总线发布集成事件
    /// </summary>
    /// <param name="evt">要发布的集成事件</param>
    Task PublishThroughEventBusAsync(IntegrationEvent evt);
}
