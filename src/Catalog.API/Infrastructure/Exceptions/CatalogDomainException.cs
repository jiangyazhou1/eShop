namespace eShop.Catalog.API.Infrastructure.Exceptions;

/// <summary>
/// 目录领域业务异常类，用于表示目录操作中的业务规则违规
/// </summary>
public class CatalogDomainException : Exception
{
    /// <summary>
    /// 初始化 CatalogDomainException 类的新实例
    /// </summary>
    public CatalogDomainException()
    { }

    /// <summary>
    /// 使用指定的错误消息初始化 CatalogDomainException 类的新实例
    /// </summary>
    /// <param name="message">描述错误的消息</param>
    public CatalogDomainException(string message)
        : base(message)
    { }

    /// <summary>
    /// 使用指定的错误消息和内部异常初始化 CatalogDomainException 类的新实例
    /// </summary>
    /// <param name="message">描述错误的消息</param>
    /// <param name="innerException">导致当前异常的异常</param>
    public CatalogDomainException(string message, Exception innerException)
        : base(message, innerException)
    { }
}
