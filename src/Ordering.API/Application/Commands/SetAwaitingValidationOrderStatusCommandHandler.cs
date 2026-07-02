namespace eShop.Ordering.API.Application.Commands;

/// <summary>
/// 设置订单状态为等待验证的命令处理器（常规处理器）
/// </summary>
public class SetAwaitingValidationOrderStatusCommandHandler : IRequestHandler<SetAwaitingValidationOrderStatusCommand, bool>
{
    /// <summary>获取订单仓库实例</summary>
    private readonly IOrderRepository _orderRepository;

    /// <summary>
    /// 初始化 SetAwaitingValidationOrderStatusCommandHandler 类的新实例
    /// </summary>
    public SetAwaitingValidationOrderStatusCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    /// <summary>
    /// 处理设置订单状态为等待验证的命令
    /// 在宽限期结束后触发此处理器
    /// </summary>
    /// <param name="command">等待验证命令</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>是否处理成功</returns>
    public async Task<bool> Handle(SetAwaitingValidationOrderStatusCommand command, CancellationToken cancellationToken)
    {
        var orderToUpdate = await _orderRepository.GetAsync(command.OrderNumber);
        if (orderToUpdate == null)
        {
            return false;
        }

        // 设置订单状态为等待验证
        orderToUpdate.SetAwaitingValidationStatus();
        return await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}


/// <summary>
/// 设置订单状态为等待验证的幂等命令处理器
/// 用于确保同一请求不会被重复处理
/// </summary>
public class SetAwaitingValidationIdentifiedOrderStatusCommandHandler : IdentifiedCommandHandler<SetAwaitingValidationOrderStatusCommand, bool>
{
    /// <summary>
    /// 初始化幂等命令处理器类的新实例
    /// </summary>
    public SetAwaitingValidationIdentifiedOrderStatusCommandHandler(
        IMediator mediator,
        IRequestManager requestManager,
        ILogger<IdentifiedCommandHandler<SetAwaitingValidationOrderStatusCommand, bool>> logger)
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
