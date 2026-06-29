namespace eShop.EventBus.Extensions;

/// <summary>
/// 泛型类型扩展方法类，用于获取泛型类型的友好名称
/// </summary>
public static class GenericTypeExtensions
{
    /// <summary>
    /// 获取类型的泛型名称字符串（例如：Dictionary`2 → Dictionary[String,Int32]）
    /// </summary>
    /// <param name="type">待处理的类型</param>
    /// <returns>格式化后的类型名称</returns>
    public static string GetGenericTypeName(this Type type)
    {
        string typeName;

        if (type.IsGenericType)
        {
            var genericTypes = string.Join(",", type.GetGenericArguments().Select(t => t.Name).ToArray());
            typeName = $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
        }
        else
        {
            typeName = type.Name;
        }

        return typeName;
    }

    public static string GetGenericTypeName(this object @object)
    {
        return @object.GetType().GetGenericTypeName();
    }
}
