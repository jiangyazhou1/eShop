using System.Diagnostics;

namespace Microsoft.AspNetCore.Hosting;

/// <summary>
/// 数据库迁移扩展类，自动执行 EF Core 迁移和种子数据填充
/// </summary>
internal static class MigrateDbContextExtensions
{
    // 追踪活动源名称，用于 OpenTelemetry
    private static readonly string ActivitySourceName = "DbMigrations";
    private static readonly ActivitySource ActivitySource = new(ActivitySourceName);

    /// <summary>
    /// 添加数据库迁移服务（不含种子数据）
    /// </summary>
    public static IServiceCollection AddMigration<TContext>(this IServiceCollection services)
        where TContext : DbContext
        => services.AddMigration<TContext>((_, _) => Task.CompletedTask);

    /// <summary>
    /// 添加数据库迁移服务，使用自定义种子数据回调
    /// </summary>
    public static IServiceCollection AddMigration<TContext>(this IServiceCollection services, Func<TContext, IServiceProvider, Task> seeder)
        where TContext : DbContext
    {
        // 启用迁移追踪
        services.AddOpenTelemetry().WithTracing(tracing => tracing.AddSource(ActivitySourceName));

        return services.AddHostedService(sp => new MigrationHostedService<TContext>(sp, seeder));
    }

    /// <summary>
    /// 添加数据库迁移服务，使用 IDbSeeder 实现作为种子数据服务
    /// </summary>
    public static IServiceCollection AddMigration<TContext, TDbSeeder>(this IServiceCollection services)
        where TContext : DbContext
        where TDbSeeder : class, IDbSeeder<TContext>
    {
        services.AddScoped<IDbSeeder<TContext>, TDbSeeder>();
        return services.AddMigration<TContext>((context, sp) => sp.GetRequiredService<IDbSeeder<TContext>>().SeedAsync(context));
    }

    /// <summary>
    /// 执行数据库迁移和种子数据填充
    /// </summary>
    private static async Task MigrateDbContextAsync<TContext>(this IServiceProvider services, Func<TContext, IServiceProvider, Task> seeder) where TContext : DbContext
    {
        using var scope = services.CreateScope();
        var scopeServices = scope.ServiceProvider;
        var logger = scopeServices.GetRequiredService<ILogger<TContext>>();
        var context = scopeServices.GetRequiredService<TContext>();

        using var activity = ActivitySource.StartActivity($"Migration operation {typeof(TContext).Name}");

        try
        {
            logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);

            var strategy = context.Database.CreateExecutionStrategy();

            // 使用 EF Core 执行策略包装迁移操作，支持容错重试
            await strategy.ExecuteAsync(() => InvokeSeeder(seeder, context, scopeServices));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);

            activity?.SetExceptionTags(ex);

            throw;
        }
    }

    /// <summary>
    /// 执行数据库迁移并调用种子数据回调
    /// </summary>
    private static async Task InvokeSeeder<TContext>(Func<TContext, IServiceProvider, Task> seeder, TContext context, IServiceProvider services)
        where TContext : DbContext
    {
        using var activity = ActivitySource.StartActivity($"Migrating {typeof(TContext).Name}");

        try
        {
            // 执行 EF Core 迁移
            await context.Database.MigrateAsync();
            // 填充种子数据
            await seeder(context, services);
        }
        catch (Exception ex)
        {
            activity?.SetExceptionTags(ex);

            throw;
        }
    }

    /// <summary>
    /// 迁移宿主服务，在应用启动时自动执行迁移
    /// </summary>
    private class MigrationHostedService<TContext>(IServiceProvider serviceProvider, Func<TContext, IServiceProvider, Task> seeder)
        : BackgroundService where TContext : DbContext
    {
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return serviceProvider.MigrateDbContextAsync(seeder);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }
    }
}

/// <summary>
/// 数据库种子数据服务接口
/// </summary>
public interface IDbSeeder<in TContext> where TContext : DbContext
{
    /// <summary>
    /// 填充种子数据
    /// </summary>
    /// <param name="context">数据库上下文</param>
    /// <returns>表示异步操作的任务</returns>
    Task SeedAsync(TContext context);
}
