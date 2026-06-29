#nullable enable

namespace eShop.Basket.API.Extensions;

/// <summary>
/// ServerCallContext 身份验证扩展方法，用于从 gRPC 调用上下文中提取用户信息
/// </summary>
internal static class ServerCallContextIdentityExtensions
{
    /// <summary>
    /// 从 ServerCallContext 中获取用户标识（"sub" 声明）
    /// </summary>
    public static string? GetUserIdentity(this ServerCallContext context) => context.GetHttpContext().User.FindFirst("sub")?.Value;

    /// <summary>
    /// 从 ServerCallContext 中获取用户名称
    /// </summary>
    public static string? GetUserName(this ServerCallContext context) => context.GetHttpContext().User.FindFirst(x => x.Type == ClaimTypes.Name)?.Value;
}
