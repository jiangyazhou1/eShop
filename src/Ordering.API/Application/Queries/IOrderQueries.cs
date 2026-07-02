namespace eShop.Ordering.API.Application.Queries;

/// <summary>
/// 订单查询接口，定义订单相关的数据查询操作
/// </summary>
public interface IOrderQueries
{
    /// <summary>
    /// 根据订单 ID 获取订单详细信息
    /// </summary>
    /// <param name="id">订单 ID</param>
    /// <returns>订单详细信息</returns>
    Task<Order> GetOrderAsync(int id);

    /// <summary>
    /// 根据用户 ID 获取用户的所有订单摘要列表
    /// </summary>
    /// <param name="userId">用户 ID</param>
    /// <returns>订单摘要集合</returns>
    Task<IEnumerable<OrderSummary>> GetOrdersFromUserAsync(string userId);

    /// <summary>
    /// 获取所有卡片类型列表
    /// </summary>
    /// <returns>卡片类型集合</returns>
    Task<IEnumerable<CardType>> GetCardTypesAsync();
}
