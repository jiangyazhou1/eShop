namespace eShop.Ordering.API.Application.DomainEventHandlers;

/// <summary>
/// 订单状态变更为库存已确认领域事件处理器
/// 当订单状态变更为库存已确认时，创建并保存集成事件
/// </summary>
public class OrderStatusChangedToStockConfirmedDomainEventHandler
                : INotificationHandler<OrderStatusChangedToStockConfirmedDomainEvent>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IBuyerRepository _buyerRepository;
    private readonly ILogger _logger;
    private readonly IOrderingIntegrationEventService _orderingIntegrationEventService;

    /// <summary>
    /// 初始化 OrderStatusChangedToStockConfirmedDomainEventHandler 类的新实例
    /// </summary>
    public OrderStatusChangedToStockConfirmedDomainEventHandler(
        IOrderRepository orderRepository,
        IBuyerRepository buyerRepository,
        ILogger<OrderStatusChangedToStockConfirmedDomainEventHandler> logger,
        IOrderingIntegrationEventService orderingIntegrationEventService)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _buyerRepository = buyerRepository ?? throw new ArgumentNullException(nameof(buyerRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _orderingIntegrationEventService = orderingIntegrationEventService;
    }

    /// <summary>
    /// 处理订单状态变更为库存已确认领域事件
    /// 记录订单状态变更日志，创建集成事件并保存
    /// </summary>
    public async Task Handle(OrderStatusChangedToStockConfirmedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        OrderingApiTrace.LogOrderStatusUpdated(_logger, domainEvent.OrderId, OrderStatus.StockConfirmed);

        var order = await _orderRepository.GetAsync(domainEvent.OrderId);
        var buyer = await _buyerRepository.FindByIdAsync(order.BuyerId.Value);

        var integrationEvent = new OrderStatusChangedToStockConfirmedIntegrationEvent(order.Id, order.OrderStatus, buyer.Name, buyer.IdentityGuid);
        await _orderingIntegrationEventService.AddAndSaveEventAsync(integrationEvent);
    }
}
