namespace eShop.Basket.API.Model;

/// <summary>
/// 客户购物车
/// </summary>
public class CustomerBasket
{
    /// <summary>
    /// 获取或设置购买者标识
    /// </summary>
    public string BuyerId { get; set; }

    /// <summary>
    /// 获取购物车商品列表
    /// </summary>
    public List<BasketItem> Items { get; set; } = [];

    /// <summary>
    /// 创建空的客户购物车
    /// </summary>
    public CustomerBasket() { }

    /// <summary>
    /// 使用指定的客户 ID 创建客户购物车
    /// </summary>
    /// <param name="customerId">客户标识</param>
    public CustomerBasket(string customerId)
    {
        BuyerId = customerId;
    }
}
