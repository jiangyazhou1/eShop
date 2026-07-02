namespace eShop.Ordering.API.Application.Commands;

/// <summary>
/// 设置订单状态为已发货的命令处理器（常规处理器）
/// </summary>
public class ShipOrderCommandHandler : IRequestHandler<ShipOrderCommand, bool>
{
    /// <summary>获取订单仓库实例</summary>
    private readonly IOrderRepository _orderRepository;

    /// <summary>
    /// 初始化 ShipOrderCommandHandler 类的新实例
    /// </summary>
    public ShipOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    /// <summary>
    /// 处理设置订单状态为已发货的命令
    /// 当管理员在应用中执行发货操作时触发此处理器
    /// </summary>
    /// <param name="command">发货命令</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否处理成功</returns>
    public async Task<bool> Handle(ShipOrderCommand command, CancellationToken cancellationToken)
    {
        var orderToUpdate = await _orderRepository.GetAsync(command.OrderNumber);
        if (orderToUpdate == null)
        {
            return false;
        }

        // 设置订单状态为已发货
        orderToUpdate.SetShippedStatus();
        return await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}


/// <summary>
/// 设置订单状态为已发货的幂等命令处理器
/// 用于确保同一请求不会被重复处理
/// </summary>
public class ShipOrderIdentifiedCommandHandler : IdentifiedCommandHandler<ShipOrderCommand, bool>
{
    /// <summary>
    /// 初始化幂等命令处理器类的新实例
    /// </summary>
    public ShipOrderIdentifiedCommandHandler(
        IMediator mediator,
        IRequestManager requestManager,
        ILogger<IdentifiedCommandHandler<ShipOrderCommand, bool>> logger)
        : base(mediator, requestManager, logger)
    {
    }

    /// <summary>
    /// 为重复请求创建结果
    /// 忽略重复处理订单的请求
    /// </summary>
    protected override bool CreateResultForDuplicateRequest()
    {
        return true; // 忽略重复处理订单的请求
    }
}
