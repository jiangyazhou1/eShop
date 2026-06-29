namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// 事件总线构建器接口，用于配置事件总线服务
/// </summary>
public interface IEventBusBuilder
{
    /// <summary>
    /// 获取服务集合，用于注册依赖注入服务
    /// </summary>
    public IServiceCollection Services { get; }
}
