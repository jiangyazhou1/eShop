namespace eShop.Catalog.API.Infrastructure.EntityConfigurations;

/// <summary>
/// 目录商品实体类型配置类，定义 CatalogItem 实体的数据库映射规则
/// </summary>
class CatalogItemEntityTypeConfiguration
    : IEntityTypeConfiguration<CatalogItem>
{
    /// <summary>
    /// 配置商品实体的数据库映射
    /// </summary>
    /// <param name="builder">实体类型构建器</param>
    public void Configure(EntityTypeBuilder<CatalogItem> builder)
    {
        // 指定表名为 Catalog
        builder.ToTable("Catalog");

        // 设置商品名称字段的最大长度为 50 个字符
        builder.Property(ci => ci.Name)
            .HasMaxLength(50);

        // 设置向量嵌入字段的类型为 384 维向量
        builder.Property(ci => ci.Embedding)
            .HasColumnType("vector(384)");

        // 配置与品牌的一对多关系
        builder.HasOne(ci => ci.CatalogBrand)
            .WithMany();

        // 配置与类型的一对多关系
        builder.HasOne(ci => ci.CatalogType)
            .WithMany();

        // 为商品名称字段创建索引
        builder.HasIndex(ci => ci.Name);
    }
}
