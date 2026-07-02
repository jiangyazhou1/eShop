namespace eShop.Ordering.API.Application.DomainEventHandlers;

/// <summary>
/// 订单启动时验证或添加购买者聚合领域事件处理器
/// 当订单启动时，验证购买者是否存在，如果不存在则创建，并验证或添加支付方法
/// </summary>
public class ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler
                    : INotificationHandler<OrderStartedDomainEvent>
{
    private readonly ILogger _logger;
    private readonly IBuyerRepository _buyerRepository;
    private readonly IOrderingIntegrationEventService _orderingIntegrationEventService;

    /// <summary>
    /// 初始化 ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler 类的新实例
    /// </summary>
    public ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler(
        ILogger<ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler> logger,
        IBuyerRepository buyerRepository,
        IOrderingIntegrationEventService orderingIntegrationEventService)
    {
        _buyerRepository = buyerRepository ?? throw new ArgumentNullException(nameof(buyerRepository));
        _orderingIntegrationEventService = orderingIntegrationEventService ?? throw new ArgumentNullException(nameof(orderingIntegrationEventService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// 处理订单启动领域事件
    /// 验证或创建购买者，验证或添加支付方法，保存购买者信息，创建订单状态变更集成事件
    /// </summary>
    public async Task Handle(OrderStartedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var cardTypeId = domainEvent.CardTypeId != 0 ? domainEvent.CardTypeId : 1;
        var buyer = await _buyerRepository.FindAsync(domainEvent.UserId);
        var buyerExisted = buyer is not null;

        if (!buyerExisted)
        {
            buyer = new Buyer(domainEvent.UserId, domainEvent.UserName);
        }

        buyer.VerifyOrAddPaymentMethod(cardTypeId,
                                        $"Payment Method on {DateTime.UtcNow}",
                                        domainEvent.CardNumber,
                                        domainEvent.CardSecurityNumber,
                                        domainEvent.CardHolderName,
                                        domainEvent.CardExpiration,
                                        domainEvent.Order.Id);

        if (!buyerExisted)
        {
            _buyerRepository.Add(buyer);
        }

        await _buyerRepository.UnitOfWork
            .SaveEntitiesAsync(cancellationToken);

        var integrationEvent = new OrderStatusChangedToSubmittedIntegrationEvent(domainEvent.Order.Id, domainEvent.Order.OrderStatus, buyer.Name, buyer.IdentityGuid);
        await _orderingIntegrationEventService.AddAndSaveEventAsync(integrationEvent);
        OrderingApiTrace.LogOrderBuyerAndPaymentValidatedOrUpdated(_logger, buyer.Id, domainEvent.Order.Id);
    }
}
