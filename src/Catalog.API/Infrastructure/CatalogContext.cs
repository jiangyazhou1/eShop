namespace eShop.Catalog.API.Infrastructure;

/// <summary>
/// 目录数据库上下文类，管理商品、品牌和类型实体的数据访问
/// </summary>
/// <remarks>
/// 添加迁移命令（在 Catalog.API 项目目录下执行）：
///
/// dotnet ef migrations add --context CatalogContext [migration-name]
/// </remarks>
public class CatalogContext : DbContext
{
    /// <summary>
    /// 使用指定的选项和配置创建 CatalogContext 实例
    /// </summary>
    /// <param name="options">数据库上下文选项</param>
    /// <param name="configuration">配置提供器</param>
    public CatalogContext(DbContextOptions<CatalogContext> options, IConfiguration configuration) : base(options)
    {
    }

    /// <summary>
    /// 获取或设置商品项集合
    /// </summary>
    public required DbSet<CatalogItem> CatalogItems { get; set; }

    /// <summary>
    /// 获取或设置品牌集合
    /// </summary>
    public required DbSet<CatalogBrand> CatalogBrands { get; set; }

    /// <summary>
    /// 获取或设置类型集合
    /// </summary>
    public required DbSet<CatalogType> CatalogTypes { get; set; }

    /// <summary>
    /// 配置数据库模型，注册实体配置和集成事件日志表
    /// </summary>
    /// <param name="builder">模型构建器</param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        // 启用 PostgreSQL vector 扩展以支持向量嵌入
        builder.HasPostgresExtension("vector");

        // 应用实体类型配置
        builder.ApplyConfiguration(new CatalogBrandEntityTypeConfiguration());
        builder.ApplyConfiguration(new CatalogTypeEntityTypeConfiguration());
        builder.ApplyConfiguration(new CatalogItemEntityTypeConfiguration());

        // 添加集成事件日志表（Outbox 模式用）
        builder.UseIntegrationEventLogs();
    }
}
