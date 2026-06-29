namespace eShop.Ordering.API.Application.Commands;

/// <summary>
/// 取消订单命令
/// </summary>
/// <param name="OrderNumber">订单号</param>
public record CancelOrderCommand(int OrderNumber) : IRequest<bool>;

