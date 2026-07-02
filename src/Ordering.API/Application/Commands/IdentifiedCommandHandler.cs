namespace eShop.Ordering.API.Application.Commands;

/// <summary>
/// 幂等命令处理器基类
/// 通过请求 ID 检测重复请求，确保同一命令只执行一次
/// </summary>
/// <typeparam name="T">实际命令类型</typeparam>
/// <typeparam name="R">命令执行结果类型</typeparam>
public abstract class IdentifiedCommandHandler<T, R> : IRequestHandler<IdentifiedCommand<T, R>, R>
    where T : IRequest<R>
{
    /// <summary>获取中介者实例</summary>
    private readonly IMediator _mediator;
    /// <summary>获取请求管理器实例（用于幂等性检查）</summary>
    private readonly IRequestManager _requestManager;
    /// <summary>获取日志记录器</summary>
    private readonly ILogger<IdentifiedCommandHandler<T, R>> _logger;

    public IdentifiedCommandHandler(
        IMediator mediator,
        IRequestManager requestManager,
        ILogger<IdentifiedCommandHandler<T, R>> logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        _mediator = mediator;
        _requestManager = requestManager;
        _logger = logger;
    }

    /// <summary>
    /// 当检测到重复请求时创建返回值
    /// 不同命令可以自定义重复请求时的行为
    /// </summary>
    protected abstract R CreateResultForDuplicateRequest();

    /// <summary>
    /// 处理带唯一标识的命令
    /// 流程：检查请求是否已存在 -> 若不存在则创建请求记录 -> 执行实际命令
    /// </summary>
    public async Task<R> Handle(IdentifiedCommand<T, R> message, CancellationToken cancellationToken)
    {
        // 检查请求 ID 是否已存在（幂等性检查）
        var alreadyExists = await _requestManager.ExistAsync(message.Id);
        if (alreadyExists)
        {
            return CreateResultForDuplicateRequest();
        }
        else
        {
            // 创建请求记录，标记该请求已处理
            await _requestManager.CreateRequestForCommandAsync<T>(message.Id);
            try
            {
                var command = message.Command;
                var commandName = command.GetGenericTypeName();
                var idProperty = string.Empty;
                var commandId = string.Empty;

                // 提取命令的关键标识属性用于日志记录
                switch (command)
                {
                    case CreateOrderCommand createOrderCommand:
                        idProperty = nameof(createOrderCommand.UserId);
                        commandId = createOrderCommand.UserId;
                        break;

                    case CancelOrderCommand cancelOrderCommand:
                        idProperty = nameof(cancelOrderCommand.OrderNumber);
                        commandId = $"{cancelOrderCommand.OrderNumber}";
                        break;

                    case ShipOrderCommand shipOrderCommand:
                        idProperty = nameof(shipOrderCommand.OrderNumber);
                        commandId = $"{shipOrderCommand.OrderNumber}";
                        break;

                    default:
                        idProperty = "Id?";
                        commandId = "n/a";
                        break;
                }

                _logger.LogInformation(
                    "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                    commandName,
                    idProperty,
                    commandId,
                    command);

                // 将实际命令发送到中介者执行对应的命令处理器
                var result = await _mediator.Send(command, cancellationToken);

                _logger.LogInformation(
                    "Command result: {@Result} - {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                    result,
                    commandName,
                    idProperty,
                    commandId,
                    command);

                return result;
            }
            catch
            {
                return default;
            }
        }
    }
}
