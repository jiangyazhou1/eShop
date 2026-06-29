namespace eShop.Ordering.Infrastructure;

/// <summary>
/// 中介者扩展方法类，提供领域事件分发功能
/// </summary>
static class MediatorExtension
{
    /// <summary>
    /// 获取数据库上下文中所有实体的领域事件，并通过中介者发布
    /// 此方法在 SaveChanges 之前调用，确保领域事件处理器与原始操作在同一事务中
    /// </summary>
    /// <param name="mediator">中介者实例</param>
    /// <param name="ctx">订单数据库上下文</param>
    public static async Task DispatchDomainEventsAsync(this IMediator mediator, OrderingContext ctx)
    {
        // 获取所有包含待处理领域事件的实体
        var domainEntities = ctx.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

        // 提取所有待发布的领域事件
        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        // 清空实体上的领域事件集合（发布前清除，防止重复触发）
        domainEntities.ToList()
            .ForEach(entity => entity.Entity.ClearDomainEvents());

        // 逐个发布领域事件，由中介者分发给对应的处理器
        foreach (var domainEvent in domainEvents)
            await mediator.Publish(domainEvent);
    }
}
