using System.Text.Json.Serialization;

namespace eShop.Ordering.Domain.AggregatesModel.OrderAggregate;

/// <summary>
/// 订单状态枚举
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OrderStatus
{
    /// <summary>
    /// 已提交
    /// </summary>
    Submitted = 1,

    /// <summary>
    /// 等待验证
    /// </summary>
    AwaitingValidation = 2,

    /// <summary>
    /// 库存已确认
    /// </summary>
    StockConfirmed = 3,

    /// <summary>
    /// 已支付
    /// </summary>
    Paid = 4,

    /// <summary>
    /// 已发货
    /// </summary>
    Shipped = 5,

    /// <summary>
    /// 已取消
    /// </summary>
    Cancelled = 6
}
