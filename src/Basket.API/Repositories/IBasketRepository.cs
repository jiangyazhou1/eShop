using eShop.Basket.API.Model;

namespace eShop.Basket.API.Repositories;

/// <summary>
/// 购物车仓库接口，定义购物车数据的持久化操作
/// </summary>
public interface IBasketRepository
{
    /// <summary>
    /// 按客户 ID 获取购物车
    /// </summary>
    /// <param name="customerId">客户标识</param>
    /// <returns>客户购物车，不存在时返回 null</returns>
    Task<CustomerBasket> GetBasketAsync(string customerId);

    /// <summary>
    /// 更新购物车数据
    /// </summary>
    /// <param name="basket">购物车实例</param>
    /// <returns>更新后的购物车</returns>
    Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket);

    /// <summary>
    /// 删除指定购物车
    /// </summary>
    /// <param name="id">购物车标识</param>
    /// <returns>删除操作是否成功</returns>
    Task<bool> DeleteBasketAsync(string id);
}
