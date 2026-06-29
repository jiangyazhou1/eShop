namespace eShop.Ordering.Infrastructure.EntityConfigurations;

/// <summary>
/// 卡片类型实体类型配置类，定义 CardType 实体的数据库映射规则
/// </summary>
class CardTypeEntityTypeConfiguration
    : IEntityTypeConfiguration<CardType>
{
    /// <summary>
    /// 配置卡片类型实体的数据库映射
    /// </summary>
    /// <param name="cardTypesConfiguration">实体类型构建器</param>
    public void Configure(EntityTypeBuilder<CardType> cardTypesConfiguration)
    {
        cardTypesConfiguration.ToTable("cardtypes");

        cardTypesConfiguration.Property(ct => ct.Id)
            .ValueGeneratedNever();

        cardTypesConfiguration.Property(ct => ct.Name)
            .HasMaxLength(200)
            .IsRequired();
    }
}
