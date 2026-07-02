namespace eShop.Ordering.API.Application.Commands;
using eShop.Ordering.API.Application.Models;

/// <summary>
/// 创建订单草稿命令，用于预览订单内容（包含商品、价格等）
/// </summary>
/// <param name="BuyerId">购买者用户ID</param>
/// <param name="Items">购物车商品列表</param>
public record CreateOrderDraftCommand(string BuyerId, IEnumerable<BasketItem> Items) : IRequest<OrderDraftDTO>;
