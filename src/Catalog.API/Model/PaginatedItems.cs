using System.Text.Json.Serialization;

namespace eShop.Catalog.API.Model;

/// <summary>
/// 分页结果包装类，包含分页元数据和数据列表
/// </summary>
/// <typeparam name="TEntity">数据实体的类型</typeparam>
public class PaginatedItems<TEntity>(int pageIndex, int pageSize, long count, IEnumerable<TEntity> data) where TEntity : class
{
    /// <summary>
    /// 获取当前页的索引（从 0 开始）
    /// </summary>
    public int PageIndex { get; } = pageIndex;

    /// <summary>
    /// 获取每页显示的记录数
    /// </summary>
    public int PageSize { get; } = pageSize;

    /// <summary>
    /// 获取符合条件的记录总数
    /// </summary>
    public long Count { get; } = count;

    /// <summary>
    /// 获取当前页的数据列表
    /// </summary>
    public IEnumerable<TEntity> Data { get;} = data;
}
