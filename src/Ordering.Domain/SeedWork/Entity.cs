namespace eShop.Ordering.Domain.Seedwork;

/// <summary>
/// 实体基类，提供 ID、领域事件管理、相等比较等基础功能
/// </summary>
public abstract class Entity
{
    int? _requestedHashCode;
    int _Id;

    /// <summary>
    /// 获取或设置实体的唯一标识符（仅子类可设置）
    /// </summary>
    public virtual int Id
    {
        get
        {
            return _Id;
        }
        protected set
        {
            _Id = value;
        }
    }

    private List<INotification> _domainEvents;

    /// <summary>
    /// 获取领域事件集合（只读）
    /// </summary>
    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();

    /// <summary>
    /// 添加领域事件到集合
    /// </summary>
    /// <param name="eventItem">领域事件</param>
    public void AddDomainEvent(INotification eventItem)
    {
        _domainEvents = _domainEvents ?? new List<INotification>();
        _domainEvents.Add(eventItem);
    }

    /// <summary>
    /// 从集合中移除指定的领域事件
    /// </summary>
    /// <param name="eventItem">要移除的事件</param>
    public void RemoveDomainEvent(INotification eventItem)
    {
        _domainEvents?.Remove(eventItem);
    }

    /// <summary>
    /// 清除所有领域事件
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents?.Clear();
    }

    /// <summary>
    /// 判断实体是否处于临时状态（尚未持久化）
    /// </summary>
    public bool IsTransient()
    {
        return this.Id == default;
    }

    /// <summary>
    /// 重写 Equals 方法，通过 ID 比较实体相等性
    /// </summary>
    public override bool Equals(object obj)
    {
        if (obj == null || !(obj is Entity))
            return false;

        if (Object.ReferenceEquals(this, obj))
            return true;

        if (this.GetType() != obj.GetType())
            return false;

        Entity item = (Entity)obj;

        if (item.IsTransient() || this.IsTransient())
            return false;
        else
            return item.Id == this.Id;
    }

    /// <summary>
    /// 重写 GetHashCode，使用 ID 计算哈希值
    /// </summary>
    public override int GetHashCode()
    {
        if (!IsTransient())
        {
            if (!_requestedHashCode.HasValue)
                _requestedHashCode = this.Id.GetHashCode() ^ 31; // XOR 实现随机分布（参考：http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx）

            return _requestedHashCode.Value;
        }
        else
            return base.GetHashCode();

    }

    /// <summary>
    /// 重载 == 运算符
    /// </summary>
    public static bool operator ==(Entity left, Entity right)
    {
        if (Object.Equals(left, null))
            return (Object.Equals(right, null)) ? true : false;
        else
            return left.Equals(right);
    }

    /// <summary>
    /// 重载 != 运算符
    /// </summary>
    public static bool operator !=(Entity left, Entity right)
    {
        return !(left == right);
    }
}
