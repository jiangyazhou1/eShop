using System.ComponentModel.DataAnnotations;

namespace eShop.Catalog.API.Model;

/// <summary>
/// 目录品牌实体类，表示商品的品牌信息
/// </summary>
public class CatalogBrand
{
    /// <summary>
    /// 使用指定的品牌名称创建新的 CatalogBrand 实例
    /// </summary>
    /// <param name="brand">品牌名称</param>
    public CatalogBrand(string brand) {
        Brand = brand;
    }

    /// <summary>
    /// 获取品牌的唯一标识符
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 获取或设置品牌名称（必填字段）
    /// </summary>
    [Required]
    public string Brand { get; set; }
}
