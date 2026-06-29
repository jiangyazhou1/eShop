namespace eShop.Catalog.API.IntegrationEvents.Events;

/// <summary>
/// 订单状态变更（已支付）集成事件
/// 当订单支付成功后触发，通知目录服务扣减相应商品库存
/// </summary>
/// <param name="OrderId">订单唯一标识符</param>
/// <param name="OrderStockItems">需要扣减库存的商品项列表</param>
public record OrderStatusChangedToPaidIntegrationEvent(int OrderId, IEnumerable<OrderStockItem> OrderStockItems) : IntegrationEvent;
