namespace eShop.Ordering.Domain.Events;

/// <summary>
/// 订单已取消领域事件，在订单被取消时触发
/// </summary>
public class OrderCancelledDomainEvent : INotification
{
    /// <summary>
    /// 获取被取消的订单实体
    /// </summary>
    public Order Order { get; }

    /// <summary>
    /// 创建订单已取消领域事件实例
    /// </summary>
    /// <param name="order">被取消的订单</param>
    public OrderCancelledDomainEvent(Order order)
    {
        Order = order;
    }
}
