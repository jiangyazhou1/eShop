namespace eShop.Ordering.Domain.Seedwork;

/// <summary>
/// 泛型仓库接口，为聚合根提供数据访问契约
/// </summary>
/// <typeparam name="T">聚合根类型</typeparam>
public interface IRepository<T> where T : IAggregateRoot
{
    /// <summary>
    /// 获取关联的工作单元实例
    /// </summary>
    IUnitOfWork UnitOfWork { get; }
}
