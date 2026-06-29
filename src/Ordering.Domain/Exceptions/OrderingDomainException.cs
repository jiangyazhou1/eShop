namespace eShop.Ordering.Domain.Exceptions;

/// <summary>
/// 订单领域异常类型
/// </summary>
public class OrderingDomainException : Exception
{
    /// <summary>
    /// 创建订单领域异常实例
    /// </summary>
    public OrderingDomainException()
    { }

    /// <summary>
    /// 使用指定消息创建订单领域异常实例
    /// </summary>
    public OrderingDomainException(string message)
        : base(message)
    { }

    /// <summary>
    /// 使用指定消息和内部异常创建订单领域异常实例
    /// </summary>
    public OrderingDomainException(string message, Exception innerException)
        : base(message, innerException)
    { }
}
