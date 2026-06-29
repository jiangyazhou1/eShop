using System.ComponentModel.DataAnnotations;

namespace eShop.Ordering.Domain.AggregatesModel.OrderAggregate;

/// <summary>
/// 订单项实体，表示订单中的单个商品
/// </summary>
public class OrderItem
    : Entity
{
    /// <summary>
    /// 获取商品名称（必填）
    /// </summary>
    [Required]
    public string ProductName { get; private set; }
    
    /// <summary>
    /// 获取商品图片 URL
    /// </summary>
    public string PictureUrl { get; private set;}
    
    /// <summary>
    /// 获取单位价格
    /// </summary>
    public decimal UnitPrice { get; private set;}
    
    /// <summary>
    /// 获取折扣金额
    /// </summary>
    public decimal Discount { get; private set; }
    
    /// <summary>
    /// 获取购买数量
    /// </summary>
    public int Units { get; private set; }

    /// <summary>
    /// 获取产品 ID
    /// </summary>
    public int ProductId { get; private set; }

    /// <summary>
    /// 私有默认构造函数，供 ORM 框架使用
    /// </summary>
    protected OrderItem() { }

    /// <summary>
    /// 创建新的订单项实例
    /// </summary>
    public OrderItem(int productId, string productName, decimal unitPrice, decimal discount, string pictureUrl, int units = 1)
    {
        if (units <= 0)
        {
            throw new OrderingDomainException("无效的数量");
        }

        if ((unitPrice * units) < discount)
        {
            throw new OrderingDomainException("订单项总价低于所应用的折扣");
        }

        ProductId = productId;

        ProductName = productName;
        UnitPrice = unitPrice;
        Discount = discount;
        Units = units;
        PictureUrl = pictureUrl;
    }
    
    /// <summary>
    /// 设置新的折扣
    /// </summary>
    public void SetNewDiscount(decimal discount)
    {
        if (discount < 0)
        {
            throw new OrderingDomainException("折扣无效");
        }

        Discount = discount;
    }

    /// <summary>
    /// 增加购买数量
    /// </summary>
    public void AddUnits(int units)
    {
        if (units < 0)
        {
            throw new OrderingDomainException("无效数量");
        }

        Units += units;
    }
}
