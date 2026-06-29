namespace eShop.Ordering.API.Application.Commands;

/// 取消订单命令处理器
/// <summary>
/// 取消订单命令处理器
/// </summary>
public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, bool>
{
    private readonly IOrderRepository _orderRepository;

    /// <summary>
    /// 初始化取消订单命令处理器
    /// </summary>
    public CancelOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    /// <summary>
    /// 处理取消订单命令
    /// 当用户在应用中执行取消订单操作时触发
    /// </summary>
    public async Task<bool> Handle(CancelOrderCommand command, CancellationToken cancellationToken)
    {
        var orderToUpdate = await _orderRepository.GetAsync(command.OrderNumber);
        if (orderToUpdate == null)
        {
            return false;
        }

        orderToUpdate.SetCancelledStatus();
        return await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}


/// 支持命令处理的幂等性
/// <summary>
/// 取消订单的幂等命令处理器
/// </summary>
public class CancelOrderIdentifiedCommandHandler : IdentifiedCommandHandler<CancelOrderCommand, bool>
{
    /// <summary>
    /// 初始化幂等命令处理器
    /// </summary>
    public CancelOrderIdentifiedCommandHandler(
        IMediator mediator,
        IRequestManager requestManager,
        ILogger<IdentifiedCommandHandler<CancelOrderCommand, bool>> logger)
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
