namespace eShop.Catalog.API.IntegrationEvents.Events;

/// <summary>
/// 订单库存拒绝集成事件
/// 当订单中部分商品库存不足时触发，携带库存不足的详细信息以便上游处理
/// </summary>
/// <param name="OrderId">订单唯一标识符</param>
/// <param name="OrderStockItems">包含库存确认结果的订单商品项列表</param>
public record OrderStockRejectedIntegrationEvent(int OrderId, List<ConfirmedOrderStockItem> OrderStockItems) : IntegrationEvent;
