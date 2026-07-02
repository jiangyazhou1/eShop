namespace eShop.Ordering.API.Infrastructure.Services;

/// <summary>
/// 身份验证服务接口，提供获取当前用户身份信息的功能
/// </summary>
public interface IIdentityService
{
    /// <summary>
    /// 获取当前用户的唯一标识符
    /// </summary>
    /// <returns>用户标识符字符串</returns>
    string GetUserIdentity();

    /// <summary>
    /// 获取当前用户的登录名称
    /// </summary>
    /// <returns>用户名称字符串</returns>
    string GetUserName();
}

