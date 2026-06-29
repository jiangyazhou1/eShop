namespace eShop.Ordering.Infrastructure.Repositories;

/// <summary>
/// 购买者仓库实现类，提供购买者聚合的持久化操作
/// </summary>
public class BuyerRepository
    : IBuyerRepository
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
    /// 初始化 BuyerRepository 类的新实例
    /// </summary>
    /// <param name="context">订单数据库上下文</param>
    public BuyerRepository(OrderingContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// 添加新的购买者；如果购买者已存在则直接返回
    /// </summary>
    public Buyer Add(Buyer buyer)
    {
        if (buyer.IsTransient())
        {
            return _context.Buyers
                .Add(buyer)
                .Entity;
        }

        return buyer;
    }

    /// <summary>
    /// 更新购买者信息
    /// </summary>
    public Buyer Update(Buyer buyer)
    {
        return _context.Buyers
                .Update(buyer)
                .Entity;
    }

    /// <summary>
    /// 按身份标识异步查找购买者（包含支付方式集合）
    /// </summary>
    public async Task<Buyer> FindAsync(string identity)
    {
        var buyer = await _context.Buyers
            .Include(b => b.PaymentMethods)
            .Where(b => b.IdentityGuid == identity)
            .SingleOrDefaultAsync();

        return buyer;
    }

    /// <summary>
    /// 按 ID 异步查找购买者（包含支付方式集合）
    /// </summary>
    public async Task<Buyer> FindByIdAsync(int id)
    {
        var buyer = await _context.Buyers
            .Include(b => b.PaymentMethods)
            .Where(b => b.Id == id)
            .SingleOrDefaultAsync();

        return buyer;
    }
}
