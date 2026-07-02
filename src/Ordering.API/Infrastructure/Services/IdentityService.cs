namespace eShop.Ordering.API.Infrastructure.Services;

/// <summary>
/// 身份验证服务实现类，通过 HTTP 上下文获取当前用户身份信息
/// </summary>
public class IdentityService(IHttpContextAccessor context) : IIdentityService
{
    /// <summary>
    /// 获取当前用户的唯一标识符
    /// </summary>
    public string GetUserIdentity()
        => context.HttpContext?.User.FindFirst("sub")?.Value;

    /// <summary>
    /// 获取当前用户的登录名称
    /// </summary>
    public string GetUserName()
        => context.HttpContext?.User.Identity?.Name;
}
