namespace eShop.Ordering.Infrastructure.Idempotency;

/// <summary>
/// 请求管理器实现类，通过数据库记录实现请求幂等性管理
/// </summary>
public class RequestManager : IRequestManager
{
    /// <summary>
    /// 获取订单数据库上下文实例
    /// </summary>
    private readonly OrderingContext _context;

    /// <summary>
    /// 初始化 RequestManager 类的新实例
    /// </summary>
    /// <param name="context">订单数据库上下文</param>
    public RequestManager(OrderingContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }


    /// <summary>
    /// 检查指定 ID 的请求是否已存在
    /// </summary>
    /// <param name="id">请求 ID</param>
    /// <returns>请求是否存在</returns>
    public async Task<bool> ExistAsync(Guid id)
    {
        var request = await _context.
            FindAsync<ClientRequest>(id);

        return request != null;
    }

    /// <summary>
    /// 为指定命令创建请求记录；如果请求已存在则抛出异常
    /// </summary>
    /// <typeparam name="T">命令类型</typeparam>
    /// <param name="id">请求 ID</param>
    public async Task CreateRequestForCommandAsync<T>(Guid id)
    {
        var exists = await ExistAsync(id);

        var request = exists ?
            throw new OrderingDomainException($"Request with {id} already exists") :
            new ClientRequest()
            {
                Id = id,
                Name = typeof(T).Name,
                Time = DateTime.UtcNow
            };

        _context.Add(request);

        await _context.SaveChangesAsync();
    }
}
