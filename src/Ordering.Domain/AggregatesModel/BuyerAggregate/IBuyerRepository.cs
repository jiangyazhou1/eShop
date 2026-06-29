namespace eShop.Ordering.Domain.AggregatesModel.BuyerAggregate;

/// <summary>
/// 购买者仓库接口，定义购买者聚合的数据访问操作
/// （领域层定义的仓库契约/接口）
/// </summary>
public interface IBuyerRepository : IRepository<Buyer>
{
    /// <summary>
    /// 添加新的购买者
    /// </summary>
    Buyer Add(Buyer buyer);

    /// <summary>
    /// 更新购买者信息
    /// </summary>
    Buyer Update(Buyer buyer);

    /// <summary>
    /// 按身份标识异步查找购买者
    /// </summary>
    Task<Buyer> FindAsync(string BuyerIdentityGuid);

    /// <summary>
    /// 按 ID 异步查找购买者
    /// </summary>
    Task<Buyer> FindByIdAsync(int id);
}

