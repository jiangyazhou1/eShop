namespace eShop.IntegrationEventLogEF.Utilities;

/// <summary>
/// 弹性事务执行类，使用 EF Core 执行策略实现容错事务
/// </summary>
public class ResilientTransaction
{
    private readonly DbContext _context;

    /// <summary>
    /// 私有构造函数，通过 New 工厂方法创建实例
    /// </summary>
    /// <param name="context">数据库上下文</param>
    private ResilientTransaction(DbContext context) =>
        _context = context ?? throw new ArgumentNullException(nameof(context));

    /// <summary>
    /// 创建弹性事务实例
    /// </summary>
    /// <param name="context">数据库上下文</param>
    /// <returns>弹性事务实例</returns>
    public static ResilientTransaction New(DbContext context) => new(context);

    /// <summary>
    /// 在弹性事务中执行操作
    /// 使用 EF Core 执行策略确保事务在遇到暂时性故障时自动重试
    /// 参考：https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency
    /// </summary>
    /// <param name="action">在事务中执行的异步操作</param>
    /// <returns>表示异步操作的任务</returns>
    public async Task ExecuteAsync(Func<Task> action)
    {
        var strategy = _context.Database.CreateExecutionStrategy();
        // 使用 EF Core 执行策略包装事务，确保操作原子性
        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            await action();
            await transaction.CommitAsync();
        });
    }
}
