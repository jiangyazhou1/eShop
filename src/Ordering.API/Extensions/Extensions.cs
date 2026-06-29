using FluentValidation;

/// <summary>
/// Ordering.API 服务注册扩展类
/// </summary>
internal static class Extensions
{
    /// <summary>
    /// 注册 Ordering.API 所需的所有应用程序服务
    /// </summary>
    /// <param name="builder">宿主应用程序构建器</param>
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;
        
        // 添加默认身份认证服务
        builder.AddDefaultAuthentication();

        // 禁用 DbContext 池化，因为 OrderingContext 没有合适的构造函数
        services.AddDbContext<OrderingContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("orderingdb"));
        });
        builder.EnrichNpgsqlDbContext<OrderingContext>();

        // 注册数据库迁移和数据种子
        services.AddMigration<OrderingContext, OrderingContextSeed>();

        // 注册集成事件日志服务
        services.AddTransient<IIntegrationEventLogService, IntegrationEventLogService<OrderingContext>>();

        // 注册订单集成事件服务
        services.AddTransient<IOrderingIntegrationEventService, OrderingIntegrationEventService>();

        // 添加 RabbitMQ 事件总线并注册事件订阅
        builder.AddRabbitMqEventBus("eventbus")
               .AddEventBusSubscriptions();

        // 注册 HTTP 上下文访问器和身份服务
        services.AddHttpContextAccessor();
        services.AddTransient<IIdentityService, IdentityService>();

        // 配置 MediatR（命令/事件处理器）
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining(typeof(Program));

            // 注册管道行为：日志 -> 验证 -> 事务
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidatorBehavior<,>));
            cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
        });

        // 注册 FluentValidation 验证器
        services.AddValidatorsFromAssemblyContaining<CancelOrderCommandValidator>();

        // 注册查询服务和仓储服务
        services.AddScoped<IOrderQueries, OrderQueries>();
        services.AddScoped<IBuyerRepository, BuyerRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IRequestManager, RequestManager>();
    }

    /// <summary>
    /// 注册事件总线订阅
    /// </summary>
    private static void AddEventBusSubscriptions(this IEventBusBuilder eventBus)
    {
        // 注册宽限期确认事件订阅
        eventBus.AddSubscription<GracePeriodConfirmedIntegrationEvent, GracePeriodConfirmedIntegrationEventHandler>();
        // 注册库存确认事件订阅
        eventBus.AddSubscription<OrderStockConfirmedIntegrationEvent, OrderStockConfirmedIntegrationEventHandler>();
        // 注册库存拒绝事件订阅
        eventBus.AddSubscription<OrderStockRejectedIntegrationEvent, OrderStockRejectedIntegrationEventHandler>();
        // 注册支付失败事件订阅
        eventBus.AddSubscription<OrderPaymentFailedIntegrationEvent, OrderPaymentFailedIntegrationEventHandler>();
        // 注册支付成功事件订阅
        eventBus.AddSubscription<OrderPaymentSucceededIntegrationEvent, OrderPaymentSucceededIntegrationEventHandler>();
    }
}
