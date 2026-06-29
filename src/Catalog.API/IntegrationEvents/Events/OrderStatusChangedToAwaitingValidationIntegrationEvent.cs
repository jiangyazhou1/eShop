namespace eShop.Catalog.API.IntegrationEvents.Events;

/// <summary>
/// 订单状态变更（等待验证）集成事件
/// 当订单提交后进入库存验证阶段时触发，包含订单 ID 和需验证的商品库存列表
/// </summary>
/// <param name="OrderId">订单唯一标识符</param>
/// <param name="OrderStockItems">需要验证库存的商品项列表</param>
public record OrderStatusChangedToAwaitingValidationIntegrationEvent(int OrderId, IEnumerable<OrderStockItem> OrderStockItems) : IntegrationEvent;
