namespace eShop.Ordering.API.Application.Commands;

/// <summary>
/// 创建订单命令
/// </summary>
/// <remarks>
/// DDD 和 CQRS 模式说明：建议实现不可变命令
/// 此处通过将所有 setter 设为 private 实现不可变性
/// 并通过构造函数在对象创建时一次性设置数据
/// </remarks>
[DataContract]
public class CreateOrderCommand
    : IRequest<bool>
{
    [DataMember]
    private readonly List<OrderItemDTO> _orderItems;

    /// <summary>获取用户ID</summary>
    [DataMember]
    public string UserId { get; private set; }

    /// <summary>获取用户名</summary>
    [DataMember]
    public string UserName { get; private set; }

    /// <summary>获取城市</summary>
    [DataMember]
    public string City { get; private set; }

    /// <summary>获取街道地址</summary>
    [DataMember]
    public string Street { get; private set; }

    /// <summary>获取省份/州</summary>
    [DataMember]
    public string State { get; private set; }

    /// <summary>获取国家</summary>
    [DataMember]
    public string Country { get; private set; }

    /// <summary>获取邮政编码</summary>
    [DataMember]
    public string ZipCode { get; private set; }

    /// <summary>获取信用卡号</summary>
    [DataMember]
    public string CardNumber { get; private set; }

    /// <summary>获取持卡人姓名</summary>
    [DataMember]
    public string CardHolderName { get; private set; }

    /// <summary>获取信用卡过期时间</summary>
    [DataMember]
    public DateTime CardExpiration { get; private set; }

    /// <summary>获取信用卡安全码</summary>
    [DataMember]
    public string CardSecurityNumber { get; private set; }

    /// <summary>获取卡片类型ID</summary>
    [DataMember]
    public int CardTypeId { get; private set; }

    /// <summary>获取订单商品列表</summary>
    [DataMember]
    public IEnumerable<OrderItemDTO> OrderItems => _orderItems;

    /// <summary>
    /// 初始化 CreateOrderCommand 的空实例
    /// </summary>
    public CreateOrderCommand()
    {
        _orderItems = new List<OrderItemDTO>();
    }

    /// <summary>
    /// 使用购物车项创建新的订单命令
    /// </summary>
    /// <param name="basketItems">购物车商品列表</param>
    /// <param name="userId">用户ID</param>
    /// <param name="userName">用户名</param>
    /// <param name="city">城市</param>
    /// <param name="street">街道</param>
    /// <param name="state">省份</param>
    /// <param name="country">国家</param>
    /// <param name="zipcode">邮政编码</param>
    /// <param name="cardNumber">信用卡号</param>
    /// <param name="cardHolderName">持卡人姓名</param>
    /// <param name="cardExpiration">过期时间</param>
    /// <param name="cardSecurityNumber">安全码</param>
    /// <param name="cardTypeId">卡片类型ID</param>
    public CreateOrderCommand(List<BasketItem> basketItems, string userId, string userName, string city, string street, string state, string country, string zipcode,
        string cardNumber, string cardHolderName, DateTime cardExpiration,
        string cardSecurityNumber, int cardTypeId)
    {
        // 将购物车项转换为订单项 DTO
        _orderItems = basketItems.ToOrderItemsDTO().ToList();
        UserId = userId;
        UserName = userName;
        City = city;
        Street = street;
        State = state;
        Country = country;
        ZipCode = zipcode;
        CardNumber = cardNumber;
        CardHolderName = cardHolderName;
        CardExpiration = cardExpiration;
        CardSecurityNumber = cardSecurityNumber;
        CardTypeId = cardTypeId;
    }
}

