namespace eShop.Ordering.Domain.SeedWork;

/// <summary>
/// 值对象基类，通过属性值相等而非标识相等
/// </summary>
public abstract class ValueObject
{
    /// <summary>
    /// 比较两个值对象是否相等
    /// </summary>
    protected static bool EqualOperator(ValueObject left, ValueObject right)
    {
        if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
        {
            return false;
        }
        return ReferenceEquals(left, null) || left.Equals(right);
    }

    /// <summary>
    /// 比较两个值对象是否不相等
    /// </summary>
    protected static bool NotEqualOperator(ValueObject left, ValueObject right)
    {
        return !(EqualOperator(left, right));
    }

    /// <summary>
    /// 获取用于相等性比较的属性组件集合（子类必须实现）
    /// </summary>
    protected abstract IEnumerable<object> GetEqualityComponents();

    /// <summary>
    /// 重写 Equals，通过属性值比较相等性
    /// </summary>
    public override bool Equals(object obj)
    {
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }

        var other = (ValueObject)obj;

        return this.GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    /// <summary>
    /// 重写 GetHashCode，基于属性值计算哈希
    /// </summary>
    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x != null ? x.GetHashCode() : 0)
            .Aggregate((x, y) => x ^ y);
    }

    /// <summary>
    /// 创建值对象的浅拷贝
    /// </summary>
    public ValueObject GetCopy()
    {
        return this.MemberwiseClone() as ValueObject;
    }
}
