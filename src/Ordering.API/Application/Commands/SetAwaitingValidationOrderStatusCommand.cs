namespace eShop.Ordering.API.Application.Commands;

/// <summary>
/// 设置订单状态为等待验证的命令
/// </summary>
/// <param name="OrderNumber">订单号</param>
public record SetAwaitingValidationOrderStatusCommand(int OrderNumber) : IRequest<bool>;
