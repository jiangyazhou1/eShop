using System.ComponentModel.DataAnnotations;

namespace eShop.WebApp.Services;

/// <summary>
/// 购物车结账信息模型，包含配送地址和支付方式数据
/// </summary>
public class BasketCheckoutInfo
{
    /// <summary>
    /// 街道地址
    /// </summary>
    [Required]
    public string? Street { get; set; }

    /// <summary>
    /// 城市
    /// </summary>
    [Required]
    public string? City { get; set; }

    /// <summary>
    /// 州/省
    /// </summary>
    [Required]
    public string? State { get; set; }

    /// <summary>
    /// 国家
    /// </summary>
    [Required]
    public string? Country { get; set; }

    /// <summary>
    /// 邮政编码
    /// </summary>
    [Required]
    public string? ZipCode { get; set; }

    /// <summary>
    /// 银行卡号
    /// </summary>
    public string? CardNumber { get; set; }

    /// <summary>
    /// 持卡人姓名
    /// </summary>
    public string? CardHolderName { get; set; }

    /// <summary>
    /// 银行卡安全码（CVV）
    /// </summary>
    public string? CardSecurityNumber { get; set; }

    /// <summary>
    /// 银行卡到期日期
    /// </summary>
    public DateTime? CardExpiration { get; set; }

    /// <summary>
    /// 卡片类型 ID（如 Visa、MasterCard 等）
    /// </summary>
    public int CardTypeId { get; set; }

    /// <summary>
    /// 购买者姓名
    /// </summary>
    public string? Buyer { get; set; }

    /// <summary>
    /// 请求追踪 ID，用于请求幂等性和日志追踪
    /// </summary>
    public Guid RequestId { get; set; }
}
