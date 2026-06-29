namespace eShop.Ordering.Domain.AggregatesModel.BuyerAggregate;

/// <summary>
/// 卡片类型记录，定义卡片的类型信息
/// </summary>
public sealed class CardType
{
    /// <summary>
    /// 获取卡片类型标识符
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// 获取卡片类型名称
    /// </summary>
    public required string Name { get; init; }
}
