namespace eShop.Ordering.API.Application.Models;

/// <summary>
/// 购物车项模型类，表示购物车中的单个商品信息
/// </summary>
public class BasketItem
{
    /// <summary>
    /// 获取购物车项的唯一标识符
    /// </summary>
    public string Id { get; init; }

    /// <summary>
    /// 获取商品 ID
    /// </summary>
    public int ProductId { get; init; }

    /// <summary>
    /// 获取商品名称
    /// </summary>
    public string ProductName { get; init; }

    /// <summary>
    /// 获取商品单价
    /// </summary>
    public decimal UnitPrice { get; init; }

    /// <summary>
    /// 获取商品原单价（用于显示折扣前的价格）
    /// </summary>
    public decimal OldUnitPrice { get; init; }

    /// <summary>
    /// 获取商品数量
    /// </summary>
    public int Quantity { get; init; }

    /// <summary>
    /// 获取商品图片 URL
    /// </summary>
    public string PictureUrl { get; init; }
}

