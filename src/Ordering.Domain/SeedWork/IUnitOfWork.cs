namespace eShop.Ordering.Domain.Seedwork;

/// <summary>
/// 工作单元接口，协调数据库变更的提交
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// 异步保存所有挂起的更改到数据库
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>受影响的行数</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 异步保存所有实体到数据库
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>保存是否成功</returns>
    Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
}
