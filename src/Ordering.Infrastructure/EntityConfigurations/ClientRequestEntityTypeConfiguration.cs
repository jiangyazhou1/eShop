namespace eShop.Ordering.Infrastructure.EntityConfigurations;

/// <summary>
/// 客户端请求实体类型配置类，定义 ClientRequest 实体的数据库映射规则
/// </summary>
class ClientRequestEntityTypeConfiguration
    : IEntityTypeConfiguration<ClientRequest>
{
    /// <summary>
    /// 配置客户端请求实体的数据库映射
    /// </summary>
    /// <param name="requestConfiguration">实体类型构建器</param>
    public void Configure(EntityTypeBuilder<ClientRequest> requestConfiguration)
    {
        requestConfiguration.ToTable("requests");
    }
}
