
namespace eShop.Ordering.Domain.Events;

/// <summary>
/// 订单已启动领域事件，在创建新订单时触发
/// </summary>
/// <param name="Order">订单实体</param>
/// <param name="UserId">用户 ID</param>
/// <param name="UserName">用户名</param>
/// <param name="CardTypeId">卡片类型 ID</param>
/// <param name="CardNumber">卡号</param>
/// <param name="CardSecurityNumber">安全码</param>
/// <param name="CardHolderName">持卡人姓名</param>
/// <param name="CardExpiration">卡片过期时间</param>
public record class OrderStartedDomainEvent(
    Order Order,
    string UserId,
    string UserName,
    int CardTypeId,
    string CardNumber,
    string CardSecurityNumber,
    string CardHolderName,
    DateTime CardExpiration) : INotification;
