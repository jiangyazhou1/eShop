using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace eShop.EventBus.Abstractions;

/// <summary>
/// 事件总线订阅信息类，存储已注册的事件类型映射与 JSON 序列化配置
/// </summary>
public class EventBusSubscriptionInfo
{
    /// <summary>
    /// 事件名称到类型实体的映射字典（事件类名 → Type）
    /// </summary>
    public Dictionary<string, Type> EventTypes { get; } = [];

    /// <summary>
    /// JSON 序列化器选项，用于消息的序列化与反序列化
    /// </summary>
    public JsonSerializerOptions JsonSerializerOptions { get; } = new(DefaultSerializerOptions);

    /// <summary>
    /// 默认的 JSON 序列化选项（支持 AOT 编译）
    /// </summary>
    internal static readonly JsonSerializerOptions DefaultSerializerOptions = new()
    {
        // 根据运行时是否默认启用反射来选择类型解析器
        TypeInfoResolver = JsonSerializer.IsReflectionEnabledByDefault ? CreateDefaultTypeResolver() : JsonTypeInfoResolver.Combine()
    };

#pragma warning disable IL2026
#pragma warning disable IL3050 // AOT 编译时可能影响功能
    /// <summary>
    /// 创建默认的 JSON 类型信息解析器
    /// </summary>
    private static IJsonTypeInfoResolver CreateDefaultTypeResolver()
        => new DefaultJsonTypeInfoResolver();
#pragma warning restore IL3050 // AOT 编译时可能影响功能
#pragma warning restore IL2026
}
