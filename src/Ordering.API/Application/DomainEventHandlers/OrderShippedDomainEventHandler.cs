namespace eShop.Ordering.API.Application.DomainEventHandlers;

/// <summary>
/// 订单已发货领域事件处理器
/// 当订单已发货时，创建并保存"订单状态变更为已发货"的集成事件
/// </summary>
public class OrderShippedDomainEventHandler
                : INotificationHandler<OrderShippedDomainEvent>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IBuyerRepository _buyerRepository;
    private readonly IOrderingIntegrationEventService _orderingIntegrationEventService;
    private readonly ILogger _logger;

    /// <summary>
    /// 初始化 OrderShippedDomainEventHandler 类的新实例
    /// </summary>
    public OrderShippedDomainEventHandler(
        IOrderRepository orderRepository,
        ILogger<OrderShippedDomainEventHandler> logger,
        IBuyerRepository buyerRepository,
        IOrderingIntegrationEventService orderingIntegrationEventService)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _buyerRepository = buyerRepository ?? throw new ArgumentNullException(nameof(buyerRepository));
        _orderingIntegrationEventService = orderingIntegrationEventService;
    }

    /// <summary>
    /// 处理订单已发货领域事件
    /// 记录订单状态变更日志，创建集成事件并保存
    /// </summary>
    public async Task Handle(OrderShippedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        OrderingApiTrace.LogOrderStatusUpdated(_logger, domainEvent.Order.Id, OrderStatus.Shipped);

        var order = await _orderRepository.GetAsync(domainEvent.Order.Id);
        var buyer = await _buyerRepository.FindByIdAsync(order.BuyerId.Value);

        var integrationEvent = new OrderStatusChangedToShippedIntegrationEvent(order.Id, order.OrderStatus, buyer.Name, buyer.IdentityGuid);
        await _orderingIntegrationEventService.AddAndSaveEventAsync(integrationEvent);
    }
}
