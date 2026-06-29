using eShop.Catalog.API.Services;

/// <summary>
/// 目录 API 服务注册扩展方法
/// </summary>
public static class Extensions
{
    /// <summary>
    /// 注册目录 API 所需的所有应用程序服务
    /// </summary>
    /// <param name="builder">主机应用程序构建器</param>
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        // 如果在构建时调用 OpenAPI 生成，则避免加载完整的数据库配置和迁移
        if (builder.Environment.IsBuild())
        {
            builder.Services.AddDbContext<CatalogContext>();
            return;
        }

        // 添加 PostgreSQL 数据库上下文，启用 vector 扩展以支持向量嵌入
        builder.AddNpgsqlDbContext<CatalogContext>("catalogdb", configureDbContextOptions: dbContextOptionsBuilder =>
        {
            dbContextOptionsBuilder.UseNpgsql(builder =>
            {
                builder.UseVector();
            });
        });

        // 注册数据库迁移和数据种子（开发环境用，生产环境不应包含此配置）
        builder.Services.AddMigration<CatalogContext, CatalogContextSeed>();

        // 添加集成事件日志服务
        builder.Services.AddTransient<IIntegrationEventLogService, IntegrationEventLogService<CatalogContext>>();

        // 添加目录集成事件服务
        builder.Services.AddTransient<ICatalogIntegrationEventService, CatalogIntegrationEventService>();

        // 配置 RabbitMQ 事件总线并注册事件订阅
        builder.AddRabbitMqEventBus("eventbus")
               // 订阅订单状态变更事件：等待验证
               .AddSubscription<OrderStatusChangedToAwaitingValidationIntegrationEvent, OrderStatusChangedToAwaitingValidationIntegrationEventHandler>()
               // 订阅订单状态变更事件：已支付
               .AddSubscription<OrderStatusChangedToPaidIntegrationEvent, OrderStatusChangedToPaidIntegrationEventHandler>();

        // 绑定目录配置选项
        builder.Services.AddOptions<CatalogOptions>()
            .BindConfiguration(nameof(CatalogOptions));

        // 配置 AI 嵌入生成器（优先使用 Ollama，其次使用 OpenAI）
        if (builder.Configuration["OllamaEnabled"] is string ollamaEnabled && bool.Parse(ollamaEnabled))
        {
            builder.AddOllamaApiClient("embedding")
                .AddEmbeddingGenerator();
        }
        else if (!string.IsNullOrWhiteSpace(builder.Configuration.GetConnectionString("textEmbeddingModel")))
        {
            builder.AddOpenAIClientFromConfiguration("textEmbeddingModel")
                .AddEmbeddingGenerator();
        }

        // 注册目录 AI 服务
        builder.Services.AddScoped<ICatalogAI, CatalogAI>();
    }
}
