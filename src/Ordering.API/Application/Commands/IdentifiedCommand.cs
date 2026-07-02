namespace eShop.Ordering.API.Application.Commands;

/// <summary>
/// 带标识的命令包装类，用于实现命令幂等性
/// 通过唯一的请求 ID 来检测重复的命令请求
/// </summary>
/// <typeparam name="T">实际命令类型</typeparam>
/// <typeparam name="R">命令返回类型</typeparam>
public class IdentifiedCommand<T, R> : IRequest<R>
    where T : IRequest<R>
{
    /// <summary>获取实际的命令对象</summary>
    public T Command { get; }
    /// <summary>获取请求的唯一标识符</summary>
    public Guid Id { get; }
    
    /// <summary>
    /// 初始化 IdentifiedCommand 类的新实例
    /// </summary>
    /// <param name="command">实际命令对象</param>
    /// <param name="id">请求唯一标识符</param>
    public IdentifiedCommand(T command, Guid id)
    {
        Command = command;
        Id = id;
    }
}
