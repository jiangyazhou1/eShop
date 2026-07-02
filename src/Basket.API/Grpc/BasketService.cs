using System.Diagnostics.CodeAnalysis;
using eShop.Basket.API.Repositories;
using eShop.Basket.API.Extensions;
using eShop.Basket.API.Model;

namespace eShop.Basket.API.Grpc;

/// <summary>
/// 购物车 gRPC 服务实现，提供购物车的查询、更新和删除操作
/// </summary>
/// <param name="repository">购物车仓库</param>
/// <param name="logger">日志记录器</param>
public class BasketService(
    IBasketRepository repository,
    ILogger<BasketService> logger) : Basket.BasketBase
{
    /// <summary>
    /// 获取购物车（允许未认证用户访问）
    /// </summary>
    [AllowAnonymous]
    public override async Task<CustomerBasketResponse> GetBasket(GetBasketRequest request, ServerCallContext context)
    {
        // 从 gRPC 上下文中获取用户身份标识
        var userId = context.GetUserIdentity();
        // 如果用户未认证，直接返回空响应（不抛出异常，因为允许匿名访问）
        if (string.IsNullOrEmpty(userId))
        {
            return new();
        }

        // 记录调试日志，追踪购物车查询请求
        if (logger.IsEnabled(LogLevel.Debug))
        {
            logger.LogDebug("Begin GetBasketById call from method {Method} for basket id {Id}", context.Method, userId);
        }

        // 从仓库获取用户购物车数据
        var data = await repository.GetBasketAsync(userId);

        // 如果购物车存在，转换为 gRPC 响应格式并返回
        if (data is not null)
        {
            return MapToCustomerBasketResponse(data);
        }

        // 购物车不存在时返回空响应
        return new();
    }

    /// <summary>
    /// 更新购物车数据
    /// </summary>
    public override async Task<CustomerBasketResponse> UpdateBasket(UpdateBasketRequest request, ServerCallContext context)
    {
        // 从 gRPC 上下文中获取用户身份标识
        var userId = context.GetUserIdentity();
        // 如果用户未认证，则抛出未授权异常
        if (string.IsNullOrEmpty(userId))
        {
            ThrowNotAuthenticated();
        }

        // 记录调试日志，便于追踪请求来源和购物车 ID
        if (logger.IsEnabled(LogLevel.Debug))
        {
            logger.LogDebug("Begin UpdateBasket call from method {Method} for basket id {Id}", context.Method, userId);
        }

        // 将 gRPC 请求数据映射为内部购物车模型
        var customerBasket = MapToCustomerBasket(userId, request);
        // 调用仓库方法执行更新操作
        var response = await repository.UpdateBasketAsync(customerBasket);
        // 如果返回值为 null，表示购物车不存在，抛出 404 异常
        if (response is null)
        {
            ThrowBasketDoesNotExist(userId);
        }

        // 将更新结果转换为 gRPC 响应格式并返回
        return MapToCustomerBasketResponse(response);
    }

    /// <summary>
    /// 删除购物车
    /// </summary>
    public override async Task<DeleteBasketResponse> DeleteBasket(DeleteBasketRequest request, ServerCallContext context)
    {
        // 从 gRPC 上下文中获取用户身份标识
        var userId = context.GetUserIdentity();
        // 如果用户未认证，则抛出未授权异常
        if (string.IsNullOrEmpty(userId))
        {
            ThrowNotAuthenticated();
        }

        // 调用仓库方法删除指定用户的购物车
        await repository.DeleteBasketAsync(userId);
        // 删除成功后返回空的响应对象
        return new();
    }

    /// <summary>
    /// 抛出未认证异常（RpcException with StatusCode.Unauthenticated）
    /// </summary>
    [DoesNotReturn]
    private static void ThrowNotAuthenticated() => throw new RpcException(new Status(StatusCode.Unauthenticated, "调用者未通过身份验证"));

    /// <summary>
    /// 抛出购物车不存在异常（RpcException with StatusCode.NotFound）
    /// </summary>
    [DoesNotReturn]
    private static void ThrowBasketDoesNotExist(string userId) => throw new RpcException(new Status(StatusCode.NotFound, $"购买者 ID 为 {userId} 的购物车不存在"));

    /// <summary>
    /// 将客户购物车模型转换为 gRPC 响应
    /// </summary>
    private static CustomerBasketResponse MapToCustomerBasketResponse(CustomerBasket customerBasket)
    {
        var response = new CustomerBasketResponse();

        // 遍历购物车中的所有商品项
        foreach (var item in customerBasket.Items)
        {
            // 将每个商品项添加到 gRPC 响应中
            response.Items.Add(new BasketItem()
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
            });
        }

        return response;
    }

    /// <summary>
    /// 将 gRPC 请求转换为客户购物车模型
    /// </summary>
    private static CustomerBasket MapToCustomerBasket(string userId, UpdateBasketRequest customerBasketRequest)
    {
        // 创建新的购物车模型，设置购买者 ID
        var response = new CustomerBasket
        {
            BuyerId = userId
        };

        // 遍历请求中的商品项
        foreach (var item in customerBasketRequest.Items)
        {
            // 将每个商品项添加到购物车中
            response.Items.Add(new()
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
            });
        }

        return response;
    }
}
