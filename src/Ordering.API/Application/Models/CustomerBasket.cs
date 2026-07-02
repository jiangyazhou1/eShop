namespace eShop.Ordering.API.Application.Models;

/// <summary>
/// 客户购物车模型类，表示用户的购物车信息
/// </summary>
public class CustomerBasket
{
    /// <summary>
    /// 获取或设置购买者 ID
    /// </summary>
    public string BuyerId { get; set; }

    /// <summary>
    /// 获取或设置购物车商品列表
    /// </summary>
    public List<BasketItem> Items { get; set; }

    /// <summary>
    /// 初始化 CustomerBasket 类的新实例
    /// </summary>
    /// <param name="buyerId">购买者 ID</param>
    /// <param name="items">购物车商品列表</param>
    public CustomerBasket(string buyerId, List<BasketItem> items)
    {
        BuyerId = buyerId;
        Items = items;
    }
}
