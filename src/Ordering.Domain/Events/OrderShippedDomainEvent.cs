namespace eShop.Ordering.Domain.Events;

/// <summary>
/// 订单已发货领域事件，在订单发货时触发
/// </summary>
public class OrderShippedDomainEvent : INotification
{
    /// <summary>
    /// 获取已发货的订单实体
    /// </summary>
    public Order Order { get; }

    /// <summary>
    /// 创建订单已发货领域事件实例
    /// </summary>
    /// <param name="order">已发货的订单</param>
    public OrderShippedDomainEvent(Order order)
    {
        Order = order;
    }
}
