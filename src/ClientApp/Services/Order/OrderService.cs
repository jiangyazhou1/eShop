using System.Net;
using eShop.ClientApp.Helpers;
using eShop.ClientApp.Models.Basket;
using eShop.ClientApp.Models.Orders;
using eShop.ClientApp.Services.Identity;
using eShop.ClientApp.Services.RequestProvider;
using eShop.ClientApp.Services.Settings;

namespace eShop.ClientApp.Services.Order;

/// <summary>
/// 订单服务实现，提供订单创建、查询、取消等功能的 HTTP 请求封装
/// </summary>
public class OrderService : IOrderService
{
    // 订单 API 的基础路径
    private const string ApiUrlBase = "api/orders";
    // API 版本参数
    private const string ApiVersion = "api-version=1.0";
    
    // 身份认证服务，用于获取用户令牌
    private readonly IIdentityService _identityService;
    // HTTP 请求提供者，用于发送 API 请求
    private readonly IRequestProvider _requestProvider;
    // 设置服务，用于获取 API 端点配置
    private readonly ISettingsService _settingsService;

    /// <summary>
    /// 初始化 OrderService 实例
    /// </summary>
    /// <param name="identityService">身份认证服务</param>
    /// <param name="settingsService">配置服务</param>
    /// <param name="requestProvider">HTTP 请求提供者</param>
    public OrderService(IIdentityService identityService, ISettingsService settingsService,
        IRequestProvider requestProvider)
    {
        _identityService = identityService;
        _settingsService = settingsService;
        _requestProvider = requestProvider;
    }

    /// <summary>
    /// 创建新订单，将订单数据通过 HTTP POST 发送到后端 API
    /// </summary>
    /// <param name="newOrder">待创建的订单对象</param>
    public async Task CreateOrderAsync(Models.Orders.Order newOrder)
    {
        // 获取用户认证令牌
        var authToken = await _identityService.GetAuthTokenAsync().ConfigureAwait(false);

        // 如果未获取到令牌，则直接返回（未认证状态无法创建订单）
        if (string.IsNullOrEmpty(authToken))
        {
            return;
        }

        // 构建完整的 API 请求地址
        var uri = $"{UriHelper.CombineUri(_settingsService.GatewayOrdersEndpointBase, ApiUrlBase)}?{ApiVersion}";

        // 发送 POST 请求创建订单，使用 x-requestid 头进行请求追踪
        var success = await _requestProvider.PostAsync(uri, newOrder, authToken, "x-requestid").ConfigureAwait(false);
    }

    /// <summary>
    /// 获取用户的所有订单列表，通过 HTTP GET 请求从后端 API 获取
    /// </summary>
    /// <returns>订单集合</returns>
    public async Task<IEnumerable<Models.Orders.Order>> GetOrdersAsync()
    {
        // 获取用户认证令牌
        var authToken = await _identityService.GetAuthTokenAsync().ConfigureAwait(false);

        // 如果未获取到令牌，返回空集合
        if (string.IsNullOrEmpty(authToken))
        {
            return Enumerable.Empty<Models.Orders.Order>();
        }

        // 构建完整的 API 请求地址
        var uri = $"{UriHelper.CombineUri(_settingsService.GatewayOrdersEndpointBase, ApiUrlBase)}?{ApiVersion}";

        // 发送 GET 请求获取订单列表
        var orders =
            await _requestProvider.GetAsync<IEnumerable<Models.Orders.Order>>(uri, authToken).ConfigureAwait(false);

        // 返回订单列表（如果为空则返回空集合）
        return orders ?? Enumerable.Empty<Models.Orders.Order>();
    }

    /// <summary>
    /// 根据订单 ID 获取单个订单的详细信息
    /// </summary>
    /// <param name="orderId">订单 ID</param>
    /// <returns>订单详情对象，若请求失败则返回空对象</returns>
    public async Task<Models.Orders.Order> GetOrderAsync(int orderId)
    {
        // 获取用户认证令牌
        var authToken = await _identityService.GetAuthTokenAsync().ConfigureAwait(false);

        // 如果未获取到令牌，返回空订单对象
        if (string.IsNullOrEmpty(authToken))
        {
            return new Models.Orders.Order();
        }

        try
        {
            // 构建包含订单 ID 的 API 请求地址
            var uri = $"{UriHelper.CombineUri(_settingsService.GatewayOrdersEndpointBase, $"{ApiUrlBase}/{orderId}")}?{ApiVersion}";

            // 发送 GET 请求获取单个订单详情
            var order =
                await _requestProvider.GetAsync<Models.Orders.Order>(uri, authToken).ConfigureAwait(false);

            return order;
        }
        catch
        {
            // 请求失败时返回空订单对象
            return new Models.Orders.Order();
        }
    }

    /// <summary>
    /// 取消指定订单，通过 HTTP PUT 请求发送取消命令到后端 API
    /// </summary>
    /// <param name="orderId">待取消的订单 ID</param>
    /// <returns>取消操作是否成功</returns>
    public async Task<bool> CancelOrderAsync(int orderId)
    {
        // 获取用户认证令牌
        var authToken = await _identityService.GetAuthTokenAsync().ConfigureAwait(false);

        // 如果未获取到令牌，返回 false 表示取消失败
        if (string.IsNullOrEmpty(authToken))
        {
            return false;
        }

        // 构建取消订单的 API 请求地址
        var uri = $"{UriHelper.CombineUri(_settingsService.GatewayOrdersEndpointBase, $"{ApiUrlBase}/cancel")}?{ApiVersion}";

        // 创建取消订单命令对象
        var cancelOrderCommand = new CancelOrderCommand(orderId);

        // 请求追踪头标识
        var header = "x-requestid";

        try
        {
            // 发送 PUT 请求取消订单
            await _requestProvider.PutAsync(uri, cancelOrderCommand, authToken, header).ConfigureAwait(false);
        }
        // 如果订单状态已变更（如在点击取消按钮前已更改），会返回 BadRequest
        catch (HttpRequestExceptionEx ex) when (ex.HttpCode == HttpStatusCode.BadRequest)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// 将订单对象映射为结账模型，用于订单提交时的数据转换
    /// </summary>
    /// <param name="order">待转换的订单对象</param>
    /// <returns>包含支付和配送信息的结账模型</returns>
    public OrderCheckout MapOrderToBasket(Models.Orders.Order order)
    {
        // 提取订单中的支付信息和配送地址，构建结账数据
        return new OrderCheckout
        {
            CardExpiration = order.CardExpiration,
            CardHolderName = order.CardHolderName,
            CardNumber = order.CardNumber,
            CardSecurityNumber = order.CardSecurityNumber,
            CardTypeId = order.CardTypeId,
            City = order.ShippingCity,
            State = order.ShippingState,
            Country = order.ShippingCountry,
            ZipCode = order.ShippingZipCode,
            Street = order.ShippingStreet
        };
    }
}
