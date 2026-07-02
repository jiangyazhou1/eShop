namespace eShop.Ordering.API.Application.Commands;

/// <summary>
/// 设置订单状态为库存确认的命令处理器（常规处理器）
/// </summary>
public class SetStockConfirmedOrderStatusCommandHandler : IRequestHandler<SetStockConfirmedOrderStatusCommand, bool>
{
    /// <summary>获取订单仓库实例</summary>
    private readonly IOrderRepository _orderRepository;

    /// <summary>
    /// 初始化 SetStockConfirmedOrderStatusCommandHandler 类的新实例
    /// </summary>
    public SetStockConfirmedOrderStatusCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    /// <summary>
    /// 处理设置订单状态为库存确认的命令
    /// 在库存服务确认库存后触发此处理器
    /// </summary>
    /// <param name="command">库存确认命令</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否处理成功</returns>
    public async Task<bool> Handle(SetStockConfirmedOrderStatusCommand command, CancellationToken cancellationToken)
    {
        // 模拟验证库存的处理时间
        await Task.Delay(10000, cancellationToken);

        var orderToUpdate = await _orderRepository.GetAsync(command.OrderNumber);
        if (orderToUpdate == null)
        {
            return false;
        }

        // 设置订单状态为库存已确认
        orderToUpdate.SetStockConfirmedStatus();
        return await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}


/// <summary>
/// 设置订单状态为库存确认的幂等命令处理器
/// 用于确保同一请求不会被重复处理
/// </summary>
public class SetStockConfirmedOrderStatusIdentifiedCommandHandler : IdentifiedCommandHandler<SetStockConfirmedOrderStatusCommand, bool>
{
    /// <summary>
    /// 初始化幂等命令处理器类的新实例
    /// </summary>
    public SetStockConfirmedOrderStatusIdentifiedCommandHandler(
        IMediator mediator,
        IRequestManager requestManager,
        ILogger<IdentifiedCommandHandler<SetStockConfirmedOrderStatusCommand, bool>> logger)
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
