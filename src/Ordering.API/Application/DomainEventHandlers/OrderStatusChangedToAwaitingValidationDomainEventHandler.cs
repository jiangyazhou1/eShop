namespace eShop.Ordering.API.Application.DomainEventHandlers;

/// <summary>
/// 订单状态变更为等待验证领域事件处理器
/// 当订单状态变更为等待验证时，创建并保存"订单状态变更为等待验证"的集成事件
/// </summary>
public class OrderStatusChangedToAwaitingValidationDomainEventHandler
                : INotificationHandler<OrderStatusChangedToAwaitingValidationDomainEvent>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger _logger;
    private readonly IBuyerRepository _buyerRepository;
    private readonly IOrderingIntegrationEventService _orderingIntegrationEventService;

    /// <summary>
    /// 初始化 OrderStatusChangedToAwaitingValidationDomainEventHandler 类的新实例
    /// </summary>
    public OrderStatusChangedToAwaitingValidationDomainEventHandler(
        IOrderRepository orderRepository,
        ILogger<OrderStatusChangedToAwaitingValidationDomainEventHandler> logger,
        IBuyerRepository buyerRepository,
        IOrderingIntegrationEventService orderingIntegrationEventService)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _buyerRepository = buyerRepository;
        _orderingIntegrationEventService = orderingIntegrationEventService;
    }

    /// <summary>
    /// 处理订单状态变更为等待验证领域事件
    /// 记录订单状态变更日志，创建集成事件并保存
    /// </summary>
    public async Task Handle(OrderStatusChangedToAwaitingValidationDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        OrderingApiTrace.LogOrderStatusUpdated(_logger, domainEvent.OrderId, OrderStatus.AwaitingValidation);

        var order = await _orderRepository.GetAsync(domainEvent.OrderId);
        var buyer = await _buyerRepository.FindByIdAsync(order.BuyerId.Value);

        var orderStockList = domainEvent.OrderItems
            .Select(orderItem => new OrderStockItem(orderItem.ProductId, orderItem.Units));

        var integrationEvent = new OrderStatusChangedToAwaitingValidationIntegrationEvent(order.Id, order.OrderStatus, buyer.Name, buyer.IdentityGuid, orderStockList);
        await _orderingIntegrationEventService.AddAndSaveEventAsync(integrationEvent);
    }
}
