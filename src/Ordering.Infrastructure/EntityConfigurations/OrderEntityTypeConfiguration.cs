namespace eShop.Ordering.Infrastructure.EntityConfigurations;

/// <summary>
/// 订单实体类型配置类，定义 Order 实体的数据库映射规则
/// </summary>
class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    /// <summary>
    /// 配置订单实体的数据库映射
    /// </summary>
    /// <param name="orderConfiguration">实体类型构建器</param>
    public void Configure(EntityTypeBuilder<Order> orderConfiguration)
    {
        orderConfiguration.ToTable("orders");

        // 忽略领域事件属性（不需要持久化到数据库）
        orderConfiguration.Ignore(b => b.DomainEvents);

        orderConfiguration.Property(o => o.Id)
            .UseHiLo("orderseq");

        // 地址值对象持久化为拥有的实体类型（EF Core 2.0+ 支持）
        orderConfiguration
            .OwnsOne(o => o.Address);

        orderConfiguration
            .Property(o => o.OrderStatus)
            .HasConversion<string>()
            .HasMaxLength(30);

        orderConfiguration
            .Property(o => o.PaymentId)
            .HasColumnName("PaymentMethodId");

        orderConfiguration.HasOne<PaymentMethod>()
            .WithMany()
            .HasForeignKey(o => o.PaymentId)
            .OnDelete(DeleteBehavior.Restrict);

        orderConfiguration.HasOne(o => o.Buyer)
            .WithMany()
            .HasForeignKey(o => o.BuyerId);
    }
}
