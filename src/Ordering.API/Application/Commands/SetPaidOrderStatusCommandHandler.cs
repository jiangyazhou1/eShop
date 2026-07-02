namespace eShop.Ordering.API.Application.Commands;

/// <summary>
/// 设置订单状态为已支付的命令处理器（常规处理器）
/// </summary>
public class SetPaidOrderStatusCommandHandler : IRequestHandler<SetPaidOrderStatusCommand, bool>
{
    /// <summary>获取订单仓库实例</summary>
    private readonly IOrderRepository _orderRepository;

    /// <summary>
    /// 初始化 SetPaidOrderStatusCommandHandler 类的新实例
    /// </summary>
    public SetPaidOrderStatusCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    /// <summary>
    /// 处理设置订单状态为已支付的命令
    /// 在支付服务确认付款后触发此处理器
    /// </summary>
    /// <param name="command">已支付命令</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否处理成功</returns>
    public async Task<bool> Handle(SetPaidOrderStatusCommand command, CancellationToken cancellationToken)
    {
        // 模拟验证支付的处理时间
        await Task.Delay(10000, cancellationToken);

        var orderToUpdate = await _orderRepository.GetAsync(command.OrderNumber);
        if (orderToUpdate == null)
        {
            return false;
        }

        orderToUpdate.SetPaidStatus();
        return await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}


/// <summary>
/// 设置订单状态为已支付的幂等命令处理器
/// 用于确保同一请求不会被重复处理
/// </summary>
public class SetPaidIdentifiedOrderStatusCommandHandler : IdentifiedCommandHandler<SetPaidOrderStatusCommand, bool>
{
    /// <summary>
    /// 初始化幂等命令处理器类的新实例
    /// </summary>
    public SetPaidIdentifiedOrderStatusCommandHandler(
        IMediator mediator,
        IRequestManager requestManager,
        ILogger<IdentifiedCommandHandler<SetPaidOrderStatusCommand, bool>> logger)
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
