using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Pgvector;

namespace eShop.Catalog.API.Model;

/// <summary>
/// 目录商品实体类，表示商品目录中的单个商品及其库存信息
/// </summary>
public class CatalogItem
{
    /// <summary>
    /// 获取商品的唯一标识符
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 获取或设置商品名称（必填字段）
    /// </summary>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// 获取或设置商品描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 获取或设置商品价格
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// 获取或设置商品图片的文件名称
    /// </summary>
    public string? PictureFileName { get; set; }

    /// <summary>
    /// 获取或设置商品所属类型的标识符
    /// </summary>
    public int CatalogTypeId { get; set; }

    /// <summary>
    /// 获取或设置商品所属的类型对象
    /// </summary>
    public CatalogType? CatalogType { get; set; }

    /// <summary>
    /// 获取或设置商品所属品牌的标识符
    /// </summary>
    public int CatalogBrandId { get; set; }

    /// <summary>
    /// 获取或设置商品所属的品牌对象
    /// </summary>
    public CatalogBrand? CatalogBrand { get; set; }

    /// <summary>
    /// 获取或设置当前可用库存数量
    /// </summary>
    public int AvailableStock { get; set; }

    /// <summary>
    /// 获取或设置库存补货阈值，当库存低于此值时触发补货
    /// </summary>
    public int RestockThreshold { get; set; }

    /// <summary>
    /// 获取或设置最大库存阈值，受仓库物理/物流限制
    /// </summary>
    public int MaxStockThreshold { get; set; }

    /// <summary>
    /// 获取或设置商品描述的可选向量嵌入，用于 AI 语义搜索
    /// </summary>
    [JsonIgnore]
    public Vector? Embedding { get; set; }

    /// <summary>
    /// 获取或设置是否正在补货中
    /// </summary>
    public bool OnReorder { get; set; }

    /// <summary>
    /// 使用指定的商品名称创建新的 CatalogItem 实例
    /// </summary>
    /// <param name="name">商品名称</param>
    public CatalogItem(string name) { Name = name; }


    /// <summary>
    /// 从库存中扣减指定数量的商品
    /// 
    /// 若库存充足，返回值等于请求数量。
    /// 若库存不足，则扣减全部可用库存并返回实际扣减数量。
    /// 调用方需自行判断返回值是否与请求数量一致。
    /// 传入负数将抛出异常。
    /// </summary>
    /// <param name="quantityDesired">请求扣减的数量</param>
    /// <returns>实际从库存中扣减的数量</returns>
    public int RemoveStock(int quantityDesired)
    {
        if (AvailableStock == 0)
        {
            throw new CatalogDomainException($"Empty stock, product item {Name} is sold out");
        }

        if (quantityDesired <= 0)
        {
            throw new CatalogDomainException($"Item units desired should be greater than zero");
        }

        // 取请求数量与可用库存中的较小值作为实际扣减量
        int removed = Math.Min(quantityDesired, this.AvailableStock);

        this.AvailableStock -= removed;

        return removed;
    }

    /// <summary>
    /// 向库存中添加指定数量的商品
    /// 若添加后超过最大库存阈值，则仅添加至阈值上限
    /// </summary>
    /// <param name="quantity">请求添加的数量</param>
    /// <returns>实际添加到库存中的数量</returns>
    public int AddStock(int quantity)
    {
        int original = this.AvailableStock;

        // 客户端请求添加的数量超过仓库可容纳的物理上限
        if ((this.AvailableStock + quantity) > this.MaxStockThreshold)
        {
            // 仅添加至最大库存阈值，超出部分暂不处理
            // 在扩展版本中可追踪剩余单位并存储到其他地方
            this.AvailableStock += (this.MaxStockThreshold - this.AvailableStock);
        }
        else
        {
            this.AvailableStock += quantity;
        }

        // 补货完成后，重置补货标志
        this.OnReorder = false;

        return this.AvailableStock - original;
    }
}
