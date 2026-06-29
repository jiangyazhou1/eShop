namespace eShop.Basket.API.Model;

/// <summary>
/// 购物车商品项
/// </summary>
public class BasketItem : IValidatableObject
{
    /// <summary>
    /// 获取或设置商品项的唯一标识符
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// 获取或设置商品的产品 ID
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// 获取或设置商品名称
    /// </summary>
    public string ProductName { get; set; }

    /// <summary>
    /// 获取或设置单位价格
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// 获取或设置原单位价格
    /// </summary>
    public decimal OldUnitPrice { get; set; }

    /// <summary>
    /// 获取或设置购买数量
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// 获取或设置商品图片 URL
    /// </summary>
    public string PictureUrl { get; set; }

    /// <summary>
    /// 验证购物车项的有效性
    /// </summary>
    /// <param name="validationContext">验证上下文</param>
    /// <returns>验证结果集合</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();

        // 数量必须大于等于 1
        if (Quantity < 1)
        {
            results.Add(new ValidationResult("无效的数量", new[] { "Quantity" }));
        }

        return results;
    }
}
