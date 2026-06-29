using eShop.IntegrationEventLogEF;

namespace eShop.Ordering.Infrastructure;

/// <remarks>
/// 添加迁移命令（在 Ordering.Infrastructure 项目目录下执行）：
///
/// dotnet ef migrations add --startup-project Ordering.API --context OrderingContext [migration-name]
/// </remarks>
/// <summary>
/// 订单数据库上下文类，管理订单、购买者等实体的数据访问
/// </summary>
public class OrderingContext : DbContext, IUnitOfWork
{
    /// <summary>获取或设置订单集合</summary>
    public DbSet<Order> Orders { get; set; }
    /// <summary>获取或设置订单项集合</summary>
    public DbSet<OrderItem> OrderItems { get; set; }
    /// <summary>获取或设置支付方式集合</summary>
    public DbSet<PaymentMethod> Payments { get; set; }
    /// <summary>获取或设置购买者集合</summary>
    public DbSet<Buyer> Buyers { get; set; }
    /// <summary>获取或设置卡片类型集合</summary>
    public DbSet<CardType> CardTypes { get; set; }

    private readonly IMediator _mediator;
    private IDbContextTransaction _currentTransaction;

    /// <summary>
    /// 使用指定的数据库上下文选项创建 OrderingContext 实例
    /// </summary>
    public OrderingContext(DbContextOptions<OrderingContext> options) : base(options) { }

    /// <summary>
    /// 获取当前活动事务
    /// </summary>
    public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;

    /// <summary>
    /// 获取是否存在活动事务
    /// </summary>
    public bool HasActiveTransaction => _currentTransaction != null;

    /// <summary>
    /// 使用指定的数据库上下文选项和中介者创建 OrderingContext 实例
    /// </summary>
    public OrderingContext(DbContextOptions<OrderingContext> options, IMediator mediator) : base(options)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));


        System.Diagnostics.Debug.WriteLine("OrderingContext::ctor ->" + this.GetHashCode());
    }

    /// <summary>
    /// 配置数据库模型，注册实体配置和集成事件日志表
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("ordering");
        modelBuilder.ApplyConfiguration(new ClientRequestEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentMethodEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OrderItemEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CardTypeEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new BuyerEntityTypeConfiguration());
        // 添加集成事件日志表（Outbox 模式用）
        modelBuilder.UseIntegrationEventLogs();
    }

    /// <summary>
    /// 保存实体更改并发布领域事件
    /// </summary>
    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        // 分发领域事件集合
        // 选择在 EF SaveChanges 提交前派发，使领域事件处理器与原始操作在同一事务中
        await _mediator.DispatchDomainEventsAsync(this);

        // 提交所有通过 DbContext 执行的更改
        _ = await base.SaveChangesAsync(cancellationToken);

        return true;
    }

    /// <summary>
    /// 开始新的数据库事务
    /// </summary>
    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        if (_currentTransaction != null) return null;

        // 使用读提交隔离级别开始事务
        _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        return _currentTransaction;
    }

    /// <summary>
    /// 提交事务
    /// </summary>
    public async Task CommitTransactionAsync(IDbContextTransaction transaction)
    {
        if (transaction == null) throw new ArgumentNullException(nameof(transaction));
        if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

        try
        {
            await SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (HasActiveTransaction)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    /// <summary>
    /// 回滚事务
    /// </summary>
    public void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            if (HasActiveTransaction)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }
}

#nullable enable
