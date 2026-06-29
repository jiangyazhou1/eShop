using System.ComponentModel.DataAnnotations;

namespace eShop.Ordering.Domain.AggregatesModel.OrderAggregate;

/// <summary>
/// 订单聚合根实体，管理订单项、状态流转和领域事件
/// </summary>
public class Order
    : Entity, IAggregateRoot
{
    /// <summary>
    /// 获取订单创建日期
    /// </summary>
    public DateTime OrderDate { get; private set; }

    /// <summary>
    /// 获取配送地址（值对象模式，由 EF Core 2.0 托管实体持久化）
    /// </summary>
    [Required]
    public Address Address { get; private set; }

    /// <summary>
    /// 获取购买者 ID
    /// </summary>
    public int? BuyerId { get; private set; }

    /// <summary>
    /// 获取购买者聚合根
    /// </summary>
    public Buyer Buyer { get; }

    /// <summary>
    /// 获取当前订单状态
    /// </summary>
    public OrderStatus OrderStatus { get; private set; }
    
    /// <summary>
    /// 获取订单描述信息
    /// </summary>
    public string Description { get; private set; }

    /// 草稿订单标志（当前未使用，可保留以备扩展）
    #pragma warning disable CS0414 // The field 'Order._isDraft' is assigned but its value is never used
    private bool _isDraft;
    #pragma warning restore CS0414

    /// 私有订单项集合，保证聚合封装性
    /// 外部只能通过 AddOrderItem() 方法添加，以便统一控制业务逻辑和校验
    private readonly List<OrderItem> _orderItems;
   
    /// <summary>
    /// 获取订单项集合（只读）
    /// </summary>
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    /// <summary>
    /// 获取支付方法 ID
    /// </summary>
    public int? PaymentId { get; private set; }

    /// <summary>
    /// 创建草稿订单（仅设置草稿标志，不初始化完整数据）
    /// </summary>
    public static Order NewDraft()
    {
        var order = new Order
        {
            _isDraft = true
        };
        return order;
    }

    /// <summary>
    /// 私有默认构造函数，供 ORM 框架使用
    /// </summary>
    protected Order()
    {
        _orderItems = new List<OrderItem>();
        _isDraft = false;
    }

    /// <summary>
    /// 创建新订单实例，设置初始状态为已提交，并添加 OrderStarted 领域事件
    /// </summary>
    /// <param name="userId">用户 ID</param>
    /// <param name="userName">用户名</param>
    /// <param name="address">配送地址</param>
    /// <param name="cardTypeId">卡片类型 ID</param>
    /// <param name="cardNumber">卡号</param>
    /// <param name="cardSecurityNumber">安全码</param>
    /// <param name="cardHolderName">持卡人姓名</param>
    /// <param name="cardExpiration">卡片过期时间</param>
    /// <param name="buyerId">购买者 ID（可选）</param>
    /// <param name="paymentMethodId">支付方法 ID（可选）</param>
    public Order(string userId, string userName, Address address, int cardTypeId, string cardNumber, string cardSecurityNumber,
            string cardHolderName, DateTime cardExpiration, int? buyerId = null, int? paymentMethodId = null) : this()
    {
        BuyerId = buyerId;
        PaymentId = paymentMethodId;
        OrderStatus = OrderStatus.Submitted;
        OrderDate = DateTime.UtcNow;
        Address = address;

        // 添加 OrderStarted 领域事件，在 DbContext.SaveChanges() 后触发/派发
        AddOrderStartedDomainEvent(userId, userName, cardTypeId, cardNumber,
                                    cardSecurityNumber, cardHolderName, cardExpiration);
    }

    /// 本方法（AddOrderItem）是向订单添加商品的唯一入口，
    /// 所有业务逻辑（折扣等）和校验由聚合根控制，以确保整个聚合的一致性。
    /// <summary>
    /// 向订单添加订单项；若商品已存在则合并数量并保留较高折扣
    /// </summary>
    /// <param name="productId">产品 ID</param>
    /// <param name="productName">产品名称</param>
    /// <param name="unitPrice">单价</param>
    /// <param name="discount">折扣金额</param>
    /// <param name="pictureUrl">图片 URL</param>
    /// <param name="units">数量</param>
    public void AddOrderItem(int productId, string productName, decimal unitPrice, decimal discount, string pictureUrl, int units = 1)
    {
        var existingOrderForProduct = _orderItems.SingleOrDefault(o => o.ProductId == productId);

        if (existingOrderForProduct != null)
        {
            // 商品已存在：若新折扣更高则更新折扣，并合并数量
            if (discount > existingOrderForProduct.Discount)
            {
                existingOrderForProduct.SetNewDiscount(discount);
            }

            existingOrderForProduct.AddUnits(units);
        }
        else
        {
            // 商品不存在：创建新的订单项
            var orderItem = new OrderItem(productId, productName, unitPrice, discount, pictureUrl, units);
            _orderItems.Add(orderItem);
        }
    }

    /// <summary>
    /// 设置支付方法验证通过后的购买者和支付 ID
    /// </summary>
    /// <param name="buyerId">购买者 ID</param>
    /// <param name="paymentId">支付方法 ID</param>
    public void SetPaymentMethodVerified(int buyerId, int paymentId)
    {
        BuyerId = buyerId;
        PaymentId = paymentId;
    }
    
    /// <summary>
    /// 将订单状态变更为等待验证（仅从 Submitted 状态可转换）
    /// </summary>
    public void SetAwaitingValidationStatus()
    {
        if (OrderStatus == OrderStatus.Submitted)
        {
            AddDomainEvent(new OrderStatusChangedToAwaitingValidationDomainEvent(Id, _orderItems));
            OrderStatus = OrderStatus.AwaitingValidation;
        }
    }

    /// <summary>
    /// 将订单状态变更为库存已确认（仅从 AwaitingValidation 状态可转换）
    /// </summary>
    public void SetStockConfirmedStatus()
    {
        if (OrderStatus == OrderStatus.AwaitingValidation)
        {
            AddDomainEvent(new OrderStatusChangedToStockConfirmedDomainEvent(Id));

            OrderStatus = OrderStatus.StockConfirmed;
            Description = "所有商品库存已确认。";
        }
    }

    /// <summary>
    /// 将订单状态变更为已支付（仅从 StockConfirmed 状态可转换）
    /// </summary>
    public void SetPaidStatus()
    {
        if (OrderStatus == OrderStatus.StockConfirmed)
        {
            AddDomainEvent(new OrderStatusChangedToPaidDomainEvent(Id, OrderItems));

            OrderStatus = OrderStatus.Paid;
            Description = "支付已完成（模拟银行，尾号 XX35071）。";
        }
    }

    /// <summary>
    /// 将订单状态变更为已发货（仅从 Paid 状态可转换）
    /// </summary>
    public void SetShippedStatus()
    {
        if (OrderStatus != OrderStatus.Paid)
        {
            StatusChangeException(OrderStatus.Shipped);
        }

        OrderStatus = OrderStatus.Shipped;
        Description = "订单已发货。";
        AddDomainEvent(new OrderShippedDomainEvent(this));
    }

    /// <summary>
    /// 将订单状态变更为已取消（Paid 或 Shipped 状态不可取消）
    /// </summary>
    public void SetCancelledStatus()
    {
        if (OrderStatus == OrderStatus.Paid ||
            OrderStatus == OrderStatus.Shipped)
        {
            StatusChangeException(OrderStatus.Cancelled);
        }

        OrderStatus = OrderStatus.Cancelled;
        Description = "订单已取消。";
        AddDomainEvent(new OrderCancelledDomainEvent(this));
    }

    /// <summary>
    /// 因库存不足将订单状态变更为已取消（仅从 AwaitingValidation 状态可转换）
    /// </summary>
    /// <param name="orderStockRejectedItems">库存不足的商品 ID 列表</param>
    public void SetCancelledStatusWhenStockIsRejected(IEnumerable<int> orderStockRejectedItems)
    {
        if (OrderStatus == OrderStatus.AwaitingValidation)
        {
            OrderStatus = OrderStatus.Cancelled;

            // 筛选出库存不足的商品名称，拼接为描述信息
            var itemsStockRejectedProductNames = OrderItems
                .Where(c => orderStockRejectedItems.Contains(c.ProductId))
                .Select(c => c.ProductName);

            var itemsStockRejectedDescription = string.Join(", ", itemsStockRejectedProductNames);
            Description = $"以下商品库存不足: ({itemsStockRejectedDescription})。";
        }
    }

    /// 创建并添加订单开始的领域事件
    private void AddOrderStartedDomainEvent(string userId, string userName, int cardTypeId, string cardNumber,
            string cardSecurityNumber, string cardHolderName, DateTime cardExpiration)
    {
        var orderStartedDomainEvent = new OrderStartedDomainEvent(this, userId, userName, cardTypeId,
                                                                    cardNumber, cardSecurityNumber,
                                                                    cardHolderName, cardExpiration);

        this.AddDomainEvent(orderStartedDomainEvent);
    }

    /// 当状态转换非法时抛出领域异常
    private void StatusChangeException(OrderStatus orderStatusToChange)
    {
        throw new OrderingDomainException($"无法将订单状态从 {OrderStatus} 变更为 {orderStatusToChange}。");
    }

    /// <summary>
    /// 计算订单总金额（所有订单项的数量 × 单价之和）
    /// </summary>
    public decimal GetTotal() => _orderItems.Sum(o => o.Units * o.UnitPrice);
}
