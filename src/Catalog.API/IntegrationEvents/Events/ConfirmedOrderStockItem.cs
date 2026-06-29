namespace eShop.Catalog.API.IntegrationEvents.Events;

/// <summary>
/// 已确认的订单库存项记录类
/// 用于记录库存验证的结果，表明某个商品是否有足够的库存
/// </summary>
/// <param name="ProductId">商品唯一标识符</param>
/// <param name="HasStock">是否有足够的库存</param>
public record ConfirmedOrderStockItem(int ProductId, bool HasStock);
