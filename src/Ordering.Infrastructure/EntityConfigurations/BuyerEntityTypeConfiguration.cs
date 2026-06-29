namespace eShop.Ordering.Infrastructure.EntityConfigurations;

/// <summary>
/// 购买者实体类型配置类，定义 Buyer 实体的数据库映射规则
/// </summary>
class BuyerEntityTypeConfiguration
    : IEntityTypeConfiguration<Buyer>
{
    /// <summary>
    /// 配置购买者实体的数据库映射
    /// </summary>
    /// <param name="buyerConfiguration">实体类型构建器</param>
    public void Configure(EntityTypeBuilder<Buyer> buyerConfiguration)
    {
        buyerConfiguration.ToTable("buyers");

        // 忽略领域事件属性（不需要持久化到数据库）
        buyerConfiguration.Ignore(b => b.DomainEvents);

        buyerConfiguration.Property(b => b.Id)
            .UseHiLo("buyerseq");

        buyerConfiguration.Property(b => b.IdentityGuid)
            .HasMaxLength(200);

        buyerConfiguration.HasIndex("IdentityGuid")
            .IsUnique(true);

        buyerConfiguration.HasMany(b => b.PaymentMethods)
            .WithOne();
    }
}
