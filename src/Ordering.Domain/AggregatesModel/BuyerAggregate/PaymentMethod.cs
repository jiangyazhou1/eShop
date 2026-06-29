using System.ComponentModel.DataAnnotations;

namespace eShop.Ordering.Domain.AggregatesModel.BuyerAggregate;

/// <summary>
/// 支付方法实体，存储卡片支付信息
/// </summary>
public class PaymentMethod : Entity
{
    [Required]
    private string _alias;
    [Required]
    private string _cardNumber;
    private string _securityNumber;
    [Required]
    private string _cardHolderName;
    private DateTime _expiration;

    private int _cardTypeId;

    /// <summary>
    /// 获取卡片类型
    /// </summary>
    public CardType CardType { get; private set; }

    /// <summary>
    /// 私有默认构造函数，供 ORM 框架使用
    /// </summary>
    protected PaymentMethod() { }

    /// <summary>
    /// 创建新的支付方法实例
    /// </summary>
    /// <param name="cardTypeId">卡片类型 ID</param>
    /// <param name="alias">卡片别名</param>
    /// <param name="cardNumber">卡号</param>
    /// <param name="securityNumber">安全码</param>
    /// <param name="cardHolderName">持卡人姓名</param>
    /// <param name="expiration">过期时间</param>
    public PaymentMethod(int cardTypeId, string alias, string cardNumber, string securityNumber, string cardHolderName, DateTime expiration)
    {
        _cardNumber = !string.IsNullOrWhiteSpace(cardNumber) ? cardNumber : throw new OrderingDomainException(nameof(cardNumber));
        _securityNumber = !string.IsNullOrWhiteSpace(securityNumber) ? securityNumber : throw new OrderingDomainException(nameof(securityNumber));
        _cardHolderName = !string.IsNullOrWhiteSpace(cardHolderName) ? cardHolderName : throw new OrderingDomainException(nameof(cardHolderName));

        // 验证卡片未过期
        if (expiration < DateTime.UtcNow)
        {
            throw new OrderingDomainException(nameof(expiration));
        }

        _alias = alias;
        _expiration = expiration;
        _cardTypeId = cardTypeId;
    }

    /// <summary>
    /// 比较支付方法是否与指定卡片信息相同
    /// </summary>
    /// <param name="cardTypeId">卡片类型 ID</param>
    /// <param name="cardNumber">卡号</param>
    /// <param name="expiration">过期时间</param>
    /// <returns>是否相同</returns>
    public bool IsEqualTo(int cardTypeId, string cardNumber, DateTime expiration)
    {
        return _cardTypeId == cardTypeId
            && _cardNumber == cardNumber
            && _expiration == expiration;
    }
}
