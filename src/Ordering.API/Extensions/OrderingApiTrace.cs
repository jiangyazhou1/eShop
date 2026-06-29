namespace eShop.Ordering.API.Extensions;

/// <summary>
/// 订单 API 结构化日志记录器，使用 ILogger 的 SourceGenerator 模式生成日志消息
/// </summary>
internal static partial class OrderingApiTrace
{
    /// <summary>
    /// 记录订单状态更新日志
    /// </summary>
    /// <param name="logger">日志记录器</param>
    /// <param name="orderId">订单 ID</param>
    /// <param name="status">新状态</param>
    [LoggerMessage(EventId = 1, EventName = "OrderStatusUpdated", Level = LogLevel.Trace, Message = "Order with Id: {OrderId} has been successfully updated to status {Status}")]
    public static partial void LogOrderStatusUpdated(ILogger logger, int orderId, OrderStatus status);

    /// <summary>
    /// 记录支付方式更新日志
    /// </summary>
    /// <param name="logger">日志记录器</param>
    /// <param name="orderId">订单 ID</param>
    /// <param name="paymentMethod">支付方式</param>
    /// <param name="id">支付方式 ID</param>
    [LoggerMessage(EventId = 2, EventName = "PaymentMethodUpdated", Level = LogLevel.Trace, Message = "Order with Id: {OrderId} has been successfully updated with a payment method {PaymentMethod} ({Id})")]
    public static partial void LogOrderPaymentMethodUpdated(ILogger logger, int orderId, string paymentMethod, int id);

    /// <summary>
    /// 记录购买者和支付方式验证/更新日志
    /// </summary>
    /// <param name="logger">日志记录器</param>
    /// <param name="buyerId">购买者 ID</param>
    /// <param name="orderId">订单 ID</param>
    [LoggerMessage(EventId = 3, EventName = "BuyerAndPaymentValidatedOrUpdated", Level = LogLevel.Trace, Message = "Buyer {BuyerId} and related payment method were validated or updated for order Id: {OrderId}.")]
    public static partial void LogOrderBuyerAndPaymentValidatedOrUpdated(ILogger logger, int buyerId, int orderId);
}
