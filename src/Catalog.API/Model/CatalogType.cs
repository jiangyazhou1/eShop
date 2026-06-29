using System.ComponentModel.DataAnnotations;

namespace eShop.Catalog.API.Model;

/// <summary>
/// 目录类型实体类，表示商品的分类类型
/// </summary>
public class CatalogType
{
    /// <summary>
    /// 使用指定的类型名称创建新的 CatalogType 实例
    /// </summary>
    /// <param name="type">类型名称</param>
    public CatalogType(string type) {
        Type = type;
    }

    /// <summary>
    /// 获取类型的唯一标识符
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 获取或设置类型名称（必填字段）
    /// </summary>
    [Required]
    public string Type { get; set; }
}
