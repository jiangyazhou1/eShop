using System.Diagnostics;

/// <summary>
/// Activity 扩展方法类，提供分布式追踪相关的辅助方法
/// </summary>
internal static class ActivityExtensions
{
    /// <summary>
    /// 将异常信息附加到 Activity 上
    /// 参考：https://opentelemetry.io/docs/specs/otel/trace/semantic_conventions/exceptions/
    /// </summary>
    /// <param name="activity">Activity 实例</param>
    /// <param name="ex">异常对象</param>
    public static void SetExceptionTags(this Activity activity, Exception ex)
    {
        if (activity is null)
        {
            return;
        }

        activity.AddTag("exception.message", ex.Message);
        activity.AddTag("exception.stacktrace", ex.ToString());
        activity.AddTag("exception.type", ex.GetType().FullName);
        activity.SetStatus(ActivityStatusCode.Error);
    }
}
