using eShop.Catalog.API.Services;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// 目录服务聚合类，用于在 API 端点中集中注入所需的服务依赖
/// 此类作为 [AsParameters] 的属性源，简化端点方法的参数声明
/// </summary>
/// <param name="context">数据库上下文</param>
/// <param name="catalogAI">AI 嵌入生成服务</param>
/// <param name="options">目录配置选项</param>
/// <param name="logger">日志记录器</param>
/// <param name="eventService">集成事件服务</param>
public class CatalogServices(
    CatalogContext context,
    [FromServices] ICatalogAI catalogAI,
    IOptions<CatalogOptions> options,
    ILogger<CatalogServices> logger,
    [FromServices] ICatalogIntegrationEventService eventService)
{
    /// <summary>
    /// 获取数据库上下文实例
    /// </summary>
    public CatalogContext Context { get; } = context;

    /// <summary>
    /// 获取 AI 嵌入生成服务实例
    /// </summary>
    public ICatalogAI CatalogAI { get; } = catalogAI;

    /// <summary>
    /// 获取目录配置选项实例
    /// </summary>
    public IOptions<CatalogOptions> Options { get; } = options;

    /// <summary>
    /// 获取日志记录器实例
    /// </summary>
    public ILogger<CatalogServices> Logger { get; } = logger;

    /// <summary>
    /// 获取集成事件服务实例
    /// </summary>
    public ICatalogIntegrationEventService EventService { get; } = eventService;
};
