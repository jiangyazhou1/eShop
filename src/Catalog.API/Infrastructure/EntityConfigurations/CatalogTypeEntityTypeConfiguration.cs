namespace eShop.Catalog.API.Infrastructure.EntityConfigurations;

/// <summary>
/// 目录类型实体类型配置类，定义 CatalogType 实体的数据库映射规则
/// </summary>
class CatalogTypeEntityTypeConfiguration
    : IEntityTypeConfiguration<CatalogType>
{
    /// <summary>
    /// 配置类型实体的数据库映射
    /// </summary>
    /// <param name="builder">实体类型构建器</param>
    public void Configure(EntityTypeBuilder<CatalogType> builder)
    {
        // 指定表名为 CatalogType
        builder.ToTable("CatalogType");

        // 设置类型名称字段的最大长度为 100 个字符
        builder.Property(cb => cb.Type)
            .HasMaxLength(100);
    }
}
