namespace eShop.Catalog.API.IntegrationEvents.Events;

/// <summary>
/// 订单库存确认集成事件
/// 当订单中所有商品的库存验证通过时触发，通知下游服务订单可进入下一步处理
/// </summary>
/// <param name="OrderId">订单唯一标识符</param>
public record OrderStockConfirmedIntegrationEvent(int OrderId) : IntegrationEvent;
