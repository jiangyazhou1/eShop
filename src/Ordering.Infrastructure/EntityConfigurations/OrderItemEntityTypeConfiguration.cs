namespace eShop.Ordering.Infrastructure.EntityConfigurations;

/// <summary>
/// 订单项实体类型配置类，定义 OrderItem 实体的数据库映射规则
/// </summary>
class OrderItemEntityTypeConfiguration
    : IEntityTypeConfiguration<OrderItem>
{
    /// <summary>
    /// 配置订单项实体的数据库映射
    /// </summary>
    /// <param name="orderItemConfiguration">实体类型构建器</param>
    public void Configure(EntityTypeBuilder<OrderItem> orderItemConfiguration)
    {
        orderItemConfiguration.ToTable("orderItems");

        // 忽略领域事件属性（不需要持久化到数据库）
        orderItemConfiguration.Ignore(b => b.DomainEvents);

        orderItemConfiguration.Property(o => o.Id)
            .UseHiLo("orderitemseq");

        orderItemConfiguration.Property<int>("OrderId");
    }
}
