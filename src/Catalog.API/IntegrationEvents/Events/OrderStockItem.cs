namespace eShop.Catalog.API.IntegrationEvents.Events;

/// <summary>
/// 订单库存项记录类
/// 表示订单中某个商品及其需要的库存数量，用于库存验证和扣减
/// </summary>
/// <param name="ProductId">商品唯一标识符</param>
/// <param name="Units">请求的数量</param>
public record OrderStockItem(int ProductId, int Units);
