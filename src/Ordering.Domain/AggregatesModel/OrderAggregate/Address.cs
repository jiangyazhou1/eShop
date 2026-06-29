using eShop.Ordering.Domain.SeedWork;

namespace eShop.Ordering.Domain.AggregatesModel.OrderAggregate;

/// <summary>
/// 地址值对象，表示订单的配送地址
/// </summary>
public class Address : ValueObject
{
    /// <summary>
    /// 获取或设置街道地址
    /// </summary>
    public string Street { get; private set; }
    /// <summary>
    /// 获取或设置城市
    /// </summary>
    public string City { get; private set; }
    /// <summary>
    /// 获取或设置州/省
    /// </summary>
    public string State { get; private set; }
    /// <summary>
    /// 获取或设置国家
    /// </summary>
    public string Country { get; private set; }
    /// <summary>
    /// 获取或设置邮政编码
    /// </summary>
    public string ZipCode { get; private set; }

    /// <summary>
    /// 私有默认构造函数，供 ORM 框架使用
    /// </summary>
    public Address() { }

    /// <summary>
    /// 创建地址实例
    /// </summary>
    public Address(string street, string city, string state, string country, string zipcode)
    {
        Street = street;
        City = city;
        State = state;
        Country = country;
        ZipCode = zipcode;
    }

    /// <summary>
    /// 获取用于相等性比较的组件
    /// </summary>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        // 逐行返回各地址组件
        yield return Street;
        yield return City;
        yield return State;
        yield return Country;
        yield return ZipCode;
    }
}
