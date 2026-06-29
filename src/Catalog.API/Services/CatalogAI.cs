using System.Diagnostics;
using Microsoft.Extensions.AI;
using Pgvector;

namespace eShop.Catalog.API.Services;

/// <summary>
/// 目录 AI 服务实现类，负责为商品生成嵌入向量以支持语义搜索
/// </summary>
public sealed class CatalogAI : ICatalogAI
{
    /// <summary>嵌入向量的维度大小</summary>
    private const int EmbeddingDimensions = 384;
    /// <summary>嵌入生成器实例（可能为 null 表示未启用）</summary>
    private readonly IEmbeddingGenerator<string, Embedding<float>>? _embeddingGenerator;
    /// <summary>获取 Web 主机环境实例</summary>
    private readonly IWebHostEnvironment _environment;
    /// <summary>获取 AI 操作用日志记录器</summary>
    private readonly ILogger _logger;

    /// <summary>
    /// 初始化 CatalogAI 类的新实例
    /// </summary>
    /// <param name="environment">Web 主机环境</param>
    /// <param name="logger">日志记录器</param>
    /// <param name="embeddingGenerator">嵌入生成器（可选）</param>
    public CatalogAI(IWebHostEnvironment environment, ILogger<CatalogAI> logger, IEmbeddingGenerator<string, Embedding<float>>? embeddingGenerator = null)
    {
        _embeddingGenerator = embeddingGenerator;
        _environment = environment;
        _logger = logger;
    }

    /// <inheritdoc/>
    public bool IsEnabled => _embeddingGenerator is not null;

    /// <inheritdoc/>
    public ValueTask<Vector?> GetEmbeddingAsync(CatalogItem item) =>
        IsEnabled ?
            GetEmbeddingAsync(CatalogItemToString(item)) :
            ValueTask.FromResult<Vector?>(null);

    /// <inheritdoc/>
    public async ValueTask<IReadOnlyList<Vector>?> GetEmbeddingsAsync(IEnumerable<CatalogItem> items)
    {
        if (IsEnabled)
        {
            long timestamp = Stopwatch.GetTimestamp();

            GeneratedEmbeddings<Embedding<float>> embeddings = await _embeddingGenerator!.GenerateAsync(items.Select(CatalogItemToString));
            var results = embeddings.Select(m => new Vector(m.Vector[0..EmbeddingDimensions])).ToList();

            if (_logger.IsEnabled(LogLevel.Trace))
            {
                // 记录生成嵌入向量的数量和时间
                _logger.LogTrace("Generated {EmbeddingsCount} embeddings in {ElapsedMilliseconds}s", results.Count, Stopwatch.GetElapsedTime(timestamp).TotalSeconds);
            }

            return results;
        }

        return null;
    }

    /// <inheritdoc/>
    public async ValueTask<Vector?> GetEmbeddingAsync(string text)
    {
        if (IsEnabled)
        {
            long timestamp = Stopwatch.GetTimestamp();

            var embedding = await _embeddingGenerator!.GenerateVectorAsync(text);
            embedding = embedding[0..EmbeddingDimensions];

            if (_logger.IsEnabled(LogLevel.Trace))
            {
                // 记录单个文本嵌入的生成时间
                _logger.LogTrace("Generated embedding in {ElapsedMilliseconds}s: '{Text}'", Stopwatch.GetElapsedTime(timestamp).TotalSeconds, text);
            }

            return new Vector(embedding);
        }

        return null;
    }

    /// <summary>
    /// 将目录项转换为文本字符串，用于生成嵌入向量
    /// </summary>
    private static string CatalogItemToString(CatalogItem item) => $"{item.Name} {item.Description}";
}
