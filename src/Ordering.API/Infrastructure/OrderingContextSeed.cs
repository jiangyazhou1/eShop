namespace eShop.Ordering.API.Infrastructure;

using eShop.Ordering.Domain.AggregatesModel.BuyerAggregate;

/// <summary>
/// 订单上下文数据库种子数据生成器
/// </summary>
public class OrderingContextSeed: IDbSeeder<OrderingContext>
{
    /// <summary>
    /// 初始化订单上下文数据库种子数据
    /// 仅当卡片类型数据不存在时添加预定义的卡片类型
    /// </summary>
    public async Task SeedAsync(OrderingContext context)
    {
        // 检查卡片类型表是否为空
        if (!context.CardTypes.Any())
        {
            // 添加预定义的卡片类型（Visa、MasterCard、Amex）
            context.CardTypes.AddRange(GetPredefinedCardTypes());

            await context.SaveChangesAsync();
        }

        await context.SaveChangesAsync();
    }

    /// <summary>
    /// 获取预定义的卡片类型列表
    /// </summary>
    private static IEnumerable<CardType> GetPredefinedCardTypes()
    {
        yield return new CardType { Id = 1, Name = "Amex" };
        yield return new CardType { Id = 2, Name = "Visa" };
        yield return new CardType { Id = 3, Name = "MasterCard" };
    }
}
