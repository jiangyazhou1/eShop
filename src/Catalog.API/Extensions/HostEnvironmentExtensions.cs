using System.Reflection;

namespace Microsoft.Extensions.Hosting;

/// <summary>
/// 主机环境扩展方法集合
/// </summary>
internal static class HostEnvironmentExtensions
{
    /// <summary>
    /// 检查当前环境是否为"Build"环境
    /// </summary>
    /// <remarks>
    /// 当应用程序通过 OpenAPI 构建时生成或通过 GetDocument.Insider 工具启动时返回 true
    /// </remarks>
    /// <param name="hostEnvironment">主机环境对象</param>
    /// <returns>如果是构建环境或入口程序集为 GetDocument.Insider 则返回 true</returns>
    public static bool IsBuild(this IHostEnvironment hostEnvironment)
    {
        // 检查环境是否为"Build"或入口程序集是否为"GetDocument.Insider"
        // 用于处理通过 GetDocument.Insider 工具进行 OpenAPI 构建时生成的场景
        return hostEnvironment.IsEnvironment("Build") || Assembly.GetEntryAssembly()?.GetName().Name == "GetDocument.Insider";
    }
}
