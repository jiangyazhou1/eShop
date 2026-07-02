namespace eShop.Ordering.API.Application.Commands;

/// <summary>
/// 设置订单状态为库存验证失败的命令
/// </summary>
/// <param name="OrderNumber">订单号</param>
/// <param name="OrderStockItems">库存不足的商品 ID 列表</param>
public record SetStockRejectedOrderStatusCommand(int OrderNumber, List<int> OrderStockItems) : IRequest<bool>;
