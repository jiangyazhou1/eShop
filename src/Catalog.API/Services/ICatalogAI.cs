using Pgvector;

namespace eShop.Catalog.API.Services;

/// <summary>
/// 目录 AI 服务接口，提供商品嵌入向量生成功能，用于语义搜索
/// </summary>
public interface ICatalogAI
{
    /// <summary>
    /// 获取 AI 系统是否已启用
    /// </summary>
    bool IsEnabled { get; }

    /// <summary>
    /// 获取指定文本的嵌入向量
    /// </summary>
    /// <param name="text">要生成嵌入向量的文本</param>
    /// <returns>嵌入向量，如果 AI 未启用则返回 null</returns>
    ValueTask<Vector?> GetEmbeddingAsync(string text);

    /// <summary>
    /// 获取指定目录项的嵌入向量
    /// </summary>
    /// <param name="item">目录项</param>
    /// <returns>嵌入向量，如果 AI 未启用则返回 null</returns>
    ValueTask<Vector?> GetEmbeddingAsync(CatalogItem item);

    /// <summary>
    /// 批量获取目录项的嵌入向量
    /// </summary>
    /// <param name="items">目录项集合</param>
    /// <returns>嵌入向量集合，如果 AI 未启用则返回 null</returns>
    ValueTask<IReadOnlyList<Vector>?> GetEmbeddingsAsync(IEnumerable<CatalogItem> items);
}
