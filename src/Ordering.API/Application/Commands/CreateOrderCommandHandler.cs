namespace eShop.Ordering.API.Application.Commands;

using eShop.Ordering.Domain.AggregatesModel.OrderAggregate;

/// <summary>
/// 创建订单命令处理器，负责处理创建订单的业务逻辑
/// </summary>
public class CreateOrderCommandHandler
    : IRequestHandler<CreateOrderCommand, bool>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IIdentityService _identityService;
    private readonly IMediator _mediator;
    private readonly IOrderingIntegrationEventService _orderingIntegrationEventService;
    private readonly ILogger<CreateOrderCommandHandler> _logger;

    /// <summary>
    /// 通过依赖注入初始化 CreateOrderCommandHandler 类的新实例
    /// </summary>
    public CreateOrderCommandHandler(IMediator mediator,
        IOrderingIntegrationEventService orderingIntegrationEventService,
        IOrderRepository orderRepository,
        IIdentityService identityService,
        ILogger<CreateOrderCommandHandler> logger)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _orderingIntegrationEventService = orderingIntegrationEventService ?? throw new ArgumentNullException(nameof(orderingIntegrationEventService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// 处理创建订单命令
    /// 1. 添加清空购物车的集成事件
    /// 2. 创建订单聚合根（含订单项）
    /// 3. 保存订单到数据库
    /// </summary>
    public async Task<bool> Handle(CreateOrderCommand message, CancellationToken cancellationToken)
    {
        // 添加集成事件：清空用户购物车
        var orderStartedIntegrationEvent = new OrderStartedIntegrationEvent(message.UserId);
        await _orderingIntegrationEventService.AddAndSaveEventAsync(orderStartedIntegrationEvent);

        // 创建订单聚合根
        // DDD 模式说明：通过 Order 聚合根的构造函数和 AddOrderItem 方法添加订单项
        // 以确保聚合内部的一致性和业务规则验证
        var address = new Address(message.Street, message.City, message.State, message.Country, message.ZipCode);
        var order = new Order(message.UserId, message.UserName, address, message.CardTypeId, message.CardNumber, message.CardSecurityNumber, message.CardHolderName, message.CardExpiration);

        // 遍历订单商品列表并添加到订单聚合根
        foreach (var item in message.OrderItems)
        {
            order.AddOrderItem(item.ProductId, item.ProductName, item.UnitPrice, item.Discount, item.PictureUrl, item.Units);
        }

        _logger.LogInformation("Creating Order - Order: {@Order}", order);

        _orderRepository.Add(order);

        return await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}

/// <summary>
/// 创建订单的幂等命令处理器
/// 用于确保同一请求不会被重复处理
/// </summary>
public class CreateOrderIdentifiedCommandHandler : IdentifiedCommandHandler<CreateOrderCommand, bool>
{
    /// <summary>
    /// 初始化 CreateOrderIdentifiedCommandHandler 类的新实例
    /// </summary>
    public CreateOrderIdentifiedCommandHandler(
        IMediator mediator,
        IRequestManager requestManager,
        ILogger<IdentifiedCommandHandler<CreateOrderCommand, bool>> logger)
        : base(mediator, requestManager, logger)
    {
    }

    /// <summary>
    /// 当检测到重复请求时创建结果
    /// 创建订单时忽略重复请求（返回 true 表示成功）
    /// </summary>
    protected override bool CreateResultForDuplicateRequest()
    {
        return true;
    }
}
