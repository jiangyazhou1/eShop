namespace eShop.Ordering.API.Application.Queries;

/// <summary>
/// 订单查询实现类，提供订单相关的数据查询功能
/// </summary>
public class OrderQueries(OrderingContext context)
    : IOrderQueries
{
    /// <summary>
    /// 根据订单 ID 获取订单详细信息
    /// </summary>
    /// <param name="id">订单 ID</param>
    /// <returns>订单详细信息</returns>
    public async Task<Order> GetOrderAsync(int id)
    {
        // 查询订单并加载订单项集合
        var order = await context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order is null)
            throw new KeyNotFoundException();

        // 将领域模型转换为视图模型
        return new Order
        {
            OrderNumber = order.Id,
            Date = order.OrderDate,
            Description = order.Description,
            City = order.Address.City,
            Country = order.Address.Country,
            State = order.Address.State,
            Street = order.Address.Street,
            Zipcode = order.Address.ZipCode,
            Status = order.OrderStatus.ToString(),
            Total = order.GetTotal(),
            OrderItems = order.OrderItems.Select(oi => new Orderitem
            {
                ProductName = oi.ProductName,
                Units = oi.Units,
                UnitPrice = (double)oi.UnitPrice,
                PictureUrl = oi.PictureUrl
            }).ToList()
        };
    }

    /// <summary>
    /// 根据用户 ID 获取用户的所有订单摘要列表
    /// </summary>
    /// <param name="userId">用户 ID</param>
    /// <returns>订单摘要集合</returns>
    public async Task<IEnumerable<OrderSummary>> GetOrdersFromUserAsync(string userId)
    {
        return await context.Orders
            .Where(o => o.Buyer.IdentityGuid == userId)
            .Select(o => new OrderSummary
            {
                OrderNumber = o.Id,
                Date = o.OrderDate,
                Status = o.OrderStatus.ToString(),
                Total =(double) o.OrderItems.Sum(oi => oi.UnitPrice* oi.Units)
            })
            .ToListAsync();
    }

    /// <summary>
    /// 获取所有卡片类型列表
    /// </summary>
    /// <returns>卡片类型集合</returns>
    public async Task<IEnumerable<CardType>> GetCardTypesAsync() =>
        await context.CardTypes.Select(c=> new CardType { Id = c.Id, Name = c.Name }).ToListAsync();
}
