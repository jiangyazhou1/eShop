using System.Text.Json.Serialization;
using eShop.Basket.API.Model;

namespace eShop.Basket.API.Repositories;

/// <summary>
/// Redis 购物车仓库实现，使用 Redis 缓存存储购物车数据
/// </summary>
public class RedisBasketRepository(ILogger<RedisBasketRepository> logger, IConnectionMultiplexer redis) : IBasketRepository
{
    private readonly IDatabase _database = redis.GetDatabase();

    // 实现说明：
    // 每个唯一购物车使用一个 Redis String 键：/basket/{id}
    // 键前缀
    private static RedisKey BasketKeyPrefix = "/basket/"u8.ToArray();
    // 注意：UTF8 编码（库的限制，待修复）- 前缀使用二进制更高效

    /// <summary>
    /// 获取指定用户的购物车键
    /// </summary>
    private static RedisKey GetBasketKey(string userId) => BasketKeyPrefix.Append(userId);

    /// <summary>
    /// 删除指定购物车
    /// </summary>
    public async Task<bool> DeleteBasketAsync(string id)
    {
        return await _database.KeyDeleteAsync(GetBasketKey(id));
    }

    /// <summary>
    /// 获取指定客户的购物车
    /// </summary>
    public async Task<CustomerBasket> GetBasketAsync(string customerId)
    {
        using var data = await _database.StringGetLeaseAsync(GetBasketKey(customerId));

        if (data is null || data.Length == 0)
        {
            return null;
        }
        return JsonSerializer.Deserialize(data.Span, BasketSerializationContext.Default.CustomerBasket);
    }

    /// <summary>
    /// 更新购物车数据
    /// </summary>
    public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
    {
        // 序列化购物车为 JSON 字节
        var json = JsonSerializer.SerializeToUtf8Bytes(basket, BasketSerializationContext.Default.CustomerBasket);
        var created = await _database.StringSetAsync(GetBasketKey(basket.BuyerId), json);

        if (!created)
        {
            logger.LogInformation("持久化商品项时发生问题");
            return null;
        }

        logger.LogInformation("购物车商品项持久化成功");
        return await GetBasketAsync(basket.BuyerId);
    }
}

/// <summary>
/// 购物车序列化上下文（AOT 兼容的 JSON 序列化器）
/// </summary>
[JsonSerializable(typeof(CustomerBasket))]
[JsonSourceGenerationOptions(PropertyNameCaseInsensitive = true)]
public partial class BasketSerializationContext : JsonSerializerContext
{

}
