namespace eShop.Ordering.API.Application.Commands;

/// <summary>
/// 设置订单状态为库存验证通过的命令
/// </summary>
/// <param name="OrderNumber">订单号</param>
public record SetStockConfirmedOrderStatusCommand(int OrderNumber) : IRequest<bool>;
