/// <summary>
/// 订单服务聚合类，用于在 API 端点中集中注入所需的服务依赖
/// </summary>
/// <param name="mediator">MediatR 中介者</param>
/// <param name="queries">订单查询服务</param>
/// <param name="identityService">身份服务</param>
/// <param name="logger">日志记录器</param>
public class OrderServices(
    IMediator mediator,
    IOrderQueries queries,
    IIdentityService identityService,
    ILogger<OrderServices> logger)
{
    /// <summary>获取 MediatR 中介者实例</summary>
    public IMediator Mediator { get; set; } = mediator;
    /// <summary>获取日志记录器实例</summary>
    public ILogger<OrderServices> Logger { get; } = logger;
    /// <summary>获取订单查询服务实例</summary>
    public IOrderQueries Queries { get; } = queries;
    /// <summary>获取身份服务实例</summary>
    public IIdentityService IdentityService { get; } = identityService;
}
