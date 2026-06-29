using System.ComponentModel.DataAnnotations;

namespace eShop.Ordering.Domain.AggregatesModel.BuyerAggregate;

/// <summary>
/// 购买者聚合根实体，管理支付方法集合
/// </summary>
public class Buyer
    : Entity, IAggregateRoot
{
    /// <summary>
    /// 获取购买者的唯一身份标识（必填）
    /// </summary>
    [Required]
    public string IdentityGuid { get; private set; }

    /// <summary>
    /// 获取购买者名称
    /// </summary>
    public string Name { get; private set; }

    private List<PaymentMethod> _paymentMethods;

    /// <summary>
    /// 获取支付方法集合（只读）
    /// </summary>
    public IEnumerable<PaymentMethod> PaymentMethods => _paymentMethods.AsReadOnly();

    /// <summary>
    /// 私有默认构造函数，供 ORM 框架使用
    /// </summary>
    protected Buyer()
    {

        _paymentMethods = new List<PaymentMethod>();
    }

    /// <summary>
    /// 创建新的购买者实例
    /// </summary>
    /// <param name="identity">唯一身份标识</param>
    /// <param name="name">购买者名称</param>
    public Buyer(string identity, string name) : this()
    {
        IdentityGuid = !string.IsNullOrWhiteSpace(identity) ? identity : throw new ArgumentNullException(nameof(identity));
        Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentNullException(nameof(name));
    }

    /// <summary>
    /// 验证现有或添加新的支付方法
    /// 如果已存在相同卡片则返回现有记录，否则创建新记录
    /// </summary>
    /// <param name="cardTypeId">卡片类型 ID</param>
    /// <param name="alias">卡片别名</param>
    /// <param name="cardNumber">卡号</param>
    /// <param name="securityNumber">安全码</param>
    /// <param name="cardHolderName">持卡人姓名</param>
    /// <param name="expiration">过期时间</param>
    /// <param name="orderId">关联订单 ID</param>
    /// <returns>支付方法实例</returns>
    public PaymentMethod VerifyOrAddPaymentMethod(
        int cardTypeId, string alias, string cardNumber,
        string securityNumber, string cardHolderName, DateTime expiration, int orderId)
    {
        // 查找是否已存在相同卡片
        var existingPayment = _paymentMethods
            .SingleOrDefault(p => p.IsEqualTo(cardTypeId, cardNumber, expiration));

        if (existingPayment != null)
        {
            // 存在则添加验证成功的领域事件
            AddDomainEvent(new BuyerAndPaymentMethodVerifiedDomainEvent(this, existingPayment, orderId));

            return existingPayment;
        }

        // 不存在则创建新支付方法
        var payment = new PaymentMethod(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, expiration);

        _paymentMethods.Add(payment);

        // 添加验证成功的领域事件
        AddDomainEvent(new BuyerAndPaymentMethodVerifiedDomainEvent(this, payment, orderId));

        return payment;
    }
}
