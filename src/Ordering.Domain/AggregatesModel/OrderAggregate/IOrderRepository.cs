namespace eShop.Ordering.Domain.AggregatesModel.OrderAggregate;

/// <summary>
/// 订单仓库接口，定义订单聚合的数据访问操作
/// （领域层定义的仓库契约/接口）
/// </summary>
public interface IOrderRepository : IRepository<Order>
{
    /// <summary>
    /// 添加新订单
    /// </summary>
    Order Add(Order order);

    /// <summary>
    /// 更新订单信息
    /// </summary>
    void Update(Order order);

    /// <summary>
    /// 按订单 ID 异步获取订单
    /// </summary>
    Task<Order> GetAsync(int orderId);
}
