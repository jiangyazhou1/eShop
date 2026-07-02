namespace eShop.Ordering.API.Application.Commands;

/// <summary>
/// 设置订单状态为已支付的命令
/// </summary>
/// <param name="OrderNumber">订单号</param>
public record SetPaidOrderStatusCommand(int OrderNumber) : IRequest<bool>;
