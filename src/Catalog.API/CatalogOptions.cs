namespace eShop.Catalog.API;

/// <summary>
/// 目录 API 的配置选项类
/// </summary>
public class CatalogOptions
{
    /// <summary>
    /// 获取或设置图片的基础 URL 地址
    /// </summary>
    public string? PicBaseUrl { get; set; }

    /// <summary>
    /// 获取或设置是否使用自定义数据
    /// </summary>
    public bool UseCustomizationData { get; set; }
}
