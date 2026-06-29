using System.ComponentModel.DataAnnotations;

namespace eShop.Ordering.Infrastructure.Idempotency;

/// <summary>
/// 客户端请求记录实体，用于幂等性检查，防止重复提交
/// </summary>
public class ClientRequest
{
    /// <summary>
    /// 获取或设置请求的唯一标识符
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 获取或设置请求名称（必填，通常为命令类型名）
    /// </summary>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// 获取或设置请求时间
    /// </summary>
    public DateTime Time { get; set; }
}
