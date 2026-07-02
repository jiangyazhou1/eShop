namespace eShop.Ordering.API.Application.DomainEventHandlers;

/// <summary>
/// 购买者和支付方法验证时更新订单领域事件处理器
/// 当购买者和支付方法已创建或验证存在时，更新订单的 BuyerId 和 PaymentId
/// </summary>
public class UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandler : INotificationHandler<BuyerAndPaymentMethodVerifiedDomainEvent>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger _logger;

    /// <summary>
    /// 初始化 UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandler 类的新实例
    /// </summary>
    public UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandler(
        IOrderRepository orderRepository,
        ILogger<UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandler> logger)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// 处理购买者和支付方法验证领域事件
    /// 更新订单的支付方法信息，设置 BuyerId 和 PaymentId（外键）
    /// </summary>
    public async Task Handle(BuyerAndPaymentMethodVerifiedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var orderToUpdate = await _orderRepository.GetAsync(domainEvent.OrderId);
        orderToUpdate.SetPaymentMethodVerified(domainEvent.Buyer.Id, domainEvent.Payment.Id);
        OrderingApiTrace.LogOrderPaymentMethodUpdated(_logger, domainEvent.OrderId, nameof(domainEvent.Payment), domainEvent.Payment.Id);
    }
}
