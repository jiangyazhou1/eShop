namespace eShop.Ordering.Infrastructure.Repositories;

/// <summary>
/// 订单仓库实现类，提供订单聚合的持久化操作
/// </summary>
public class OrderRepository
    : IOrderRepository
{
    /// <summary>
    /// 获取订单数据库上下文实例
    /// </summary>
    private readonly OrderingContext _context;

    /// <summary>
    /// 获取关联的工作单元实例
    /// </summary>
    public IUnitOfWork UnitOfWork => _context;

    /// <summary>
    /// 初始化 OrderRepository 类的新实例
    /// </summary>
    /// <param name="context">订单数据库上下文</param>
    public OrderRepository(OrderingContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// 添加新订单
    /// </summary>
    public Order Add(Order order)
    {
        return _context.Orders.Add(order).Entity;

    }

    /// <summary>
    /// 按订单 ID 异步获取订单（含订单项集合）
    /// </summary>
    public async Task<Order> GetAsync(int orderId)
    {
        var order = await _context.Orders.FindAsync(orderId);

        if (order != null)
        {
            // 使用 EF Core 的激进入口策略加载订单项集合
            await _context.Entry(order)
                .Collection(i => i.OrderItems).LoadAsync();
        }

        return order;
    }

    /// <summary>
    /// 更新订单状态为已修改（跟踪变更）
    /// </summary>
    public void Update(Order order)
    {
        _context.Entry(order).State = EntityState.Modified;
    }
}
