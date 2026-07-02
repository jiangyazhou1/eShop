namespace eShop.Ordering.API.Application.Commands;

using eShop.Ordering.API.Extensions;
using eShop.Ordering.Domain.AggregatesModel.OrderAggregate;

/// <summary>
/// 创建订单草稿命令处理器，用于处理订单草稿预览请求
/// </summary>
public class CreateOrderDraftCommandHandler
    : IRequestHandler<CreateOrderDraftCommand, OrderDraftDTO>
{
    /// <summary>
    /// 处理创建订单草稿命令
    /// 生成订单草稿（不实际保存），包含商品列表和总价
    /// </summary>
    public Task<OrderDraftDTO> Handle(CreateOrderDraftCommand message, CancellationToken cancellationToken)
    {
        // 创建空白的订单草稿
        var order = Order.NewDraft();
        // 将购物车商品转换为订单项 DTO
        var orderItems = message.Items.Select(i => i.ToOrderItemDTO());
        // 将订单项添加到订单草稿
        foreach (var item in orderItems)
        {
            order.AddOrderItem(item.ProductId, item.ProductName, item.UnitPrice, item.Discount, item.PictureUrl, item.Units);
        }

        return Task.FromResult(OrderDraftDTO.FromOrder(order));
    }
}

/// <summary>
/// 订单草稿数据传输对象，包含订单项列表和总价
/// </summary>
public record OrderDraftDTO
{
    /// <summary>获取订单项列表</summary>
    public IEnumerable<OrderItemDTO> OrderItems { get; init; }
    /// <summary>获取订单总价</summary>
    public decimal Total { get; init; }

    /// <summary>
    /// 从订单聚合根创建订单草稿 DTO
    /// </summary>
    public static OrderDraftDTO FromOrder(Order order)
    {
        return new OrderDraftDTO()
        {
            OrderItems = order.OrderItems.Select(oi => new OrderItemDTO
            {
                Discount = oi.Discount,
                ProductId = oi.ProductId,
                UnitPrice = oi.UnitPrice,
                PictureUrl = oi.PictureUrl,
                Units = oi.Units,
                ProductName = oi.ProductName
            }),
            Total = order.GetTotal()
        };
    }
}

/// <summary>
/// 订单项数据传输对象
/// </summary>
public record OrderItemDTO
{
    /// <summary>获取商品ID</summary>
    public int ProductId { get; init; }
    /// <summary>获取商品名称</summary>
    public string ProductName { get; init; }
    /// <summary>获取单价</summary>
    public decimal UnitPrice { get; init; }
    /// <summary>获取折扣金额</summary>
    public decimal Discount { get; init; }
    /// <summary>获取购买数量</summary>
    public int Units { get; init; }
    /// <summary>获取商品图片URL</summary>
    public string PictureUrl { get; init; }
}
