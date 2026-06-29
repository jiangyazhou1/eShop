namespace eShop.Catalog.API.Infrastructure.EntityConfigurations;

/// <summary>
/// 目录品牌实体类型配置类，定义 CatalogBrand 实体的数据库映射规则
/// </summary>
class CatalogBrandEntityTypeConfiguration
    : IEntityTypeConfiguration<CatalogBrand>
{
    /// <summary>
    /// 配置品牌实体的数据库映射
    /// </summary>
    /// <param name="builder">实体类型构建器</param>
    public void Configure(EntityTypeBuilder<CatalogBrand> builder)
    {
        // 指定表名为 CatalogBrand
        builder.ToTable("CatalogBrand");

        // 设置品牌名称字段的最大长度为 100 个字符
        builder.Property(cb => cb.Brand)
            .HasMaxLength(100);
    }
}
