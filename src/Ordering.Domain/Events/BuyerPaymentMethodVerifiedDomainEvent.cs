namespace eShop.Ordering.Domain.Events;

/// <summary>
/// 购买者和支付方法已验证领域事件，在购买者和支付方法验证通过后触发
/// </summary>
public class BuyerAndPaymentMethodVerifiedDomainEvent
    : INotification
{
    /// <summary>
    /// 获取购买者实体
    /// </summary>
    public Buyer Buyer { get; private set; }

    /// <summary>
    /// 获取支付方法实体
    /// </summary>
    public PaymentMethod Payment { get; private set; }

    /// <summary>
    /// 获取关联的订单 ID
    /// </summary>
    public int OrderId { get; private set; }

    /// <summary>
    /// 创建购买者和支付方法已验证领域事件实例
    /// </summary>
    /// <param name="buyer">购买者</param>
    /// <param name="payment">支付方法</param>
    /// <param name="orderId">订单 ID</param>
    public BuyerAndPaymentMethodVerifiedDomainEvent(Buyer buyer, PaymentMethod payment, int orderId)
    {
        Buyer = buyer;
        Payment = payment;
        OrderId = orderId;
    }
}
