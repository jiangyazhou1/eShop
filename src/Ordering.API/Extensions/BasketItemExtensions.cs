namespace eShop.Ordering.API.Extensions;

/// <summary>
/// 购物车项扩展方法类，提供购物车项到订单项 DTO 的转换功能
/// </summary>
public static class BasketItemExtensions
{
    /// <summary>
    /// 将购物车项集合转换为订单项 DTO 集合
    /// </summary>
    /// <param name="basketItems">购物车项集合</param>
    /// <returns>订单项 DTO 集合</returns>
    public static IEnumerable<OrderItemDTO> ToOrderItemsDTO(this IEnumerable<BasketItem> basketItems)
    {
        foreach (var item in basketItems)
        {
            yield return item.ToOrderItemDTO();
        }
    }

    /// <summary>
    /// 将购物车项转换为订单项 DTO
    /// </summary>
    /// <param name="item">购物车项</param>
    /// <returns>订单项 DTO</returns>
    public static OrderItemDTO ToOrderItemDTO(this BasketItem item)
    {
        return new OrderItemDTO()
        {
            ProductId = item.ProductId,
            ProductName = item.ProductName,
            PictureUrl = item.PictureUrl,
            UnitPrice = item.UnitPrice,
            Units = item.Quantity
        };
    }
}
