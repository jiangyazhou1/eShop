namespace eShop.Ordering.Infrastructure.Idempotency;

/// <summary>
/// 请求管理器接口，提供请求幂等性管理功能
/// </summary>
public interface IRequestManager
{
    /// <summary>
    /// 检查请求是否已存在
    /// </summary>
    /// <param name="id">请求 ID</param>
    /// <returns>请求是否存在</returns>
    Task<bool> ExistAsync(Guid id);

    /// <summary>
    /// 为指定命令类型创建请求记录
    /// </summary>
    /// <typeparam name="T">命令类型</typeparam>
    /// <param name="id">请求 ID</param>
    Task CreateRequestForCommandAsync<T>(Guid id);
}
