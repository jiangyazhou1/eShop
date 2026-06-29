namespace eShop.Catalog.API.IntegrationEvents.Events;

/// <summary>
/// 商品价格变更集成事件
/// 当商品价格发生变化时触发，通知其他微服务或外部系统
/// 注意：集成事件命名使用过去时态，表示已发生的事实
/// </summary>
/// <param name="ProductId">商品唯一标识符</param>
/// <param name="NewPrice">新价格</param>
/// <param name="OldPrice">旧价格</param>
public record ProductPriceChangedIntegrationEvent(int ProductId, decimal NewPrice, decimal OldPrice) : IntegrationEvent;
