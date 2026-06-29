namespace eShop.Ordering.API.Extensions;

/// <summary>
/// LINQ Select 扩展方法集合，提供安全的 Select 操作，捕获转换异常
/// </summary>
public static class LinqSelectExtensions
{
    /// <summary>
    /// 对序列中的每个元素执行选择器函数，捕获异常而不中断迭代
    /// </summary>
    /// <typeparam name="TSource">源元素类型</typeparam>
    /// <typeparam name="TResult">结果类型</typeparam>
    /// <param name="enumerable">源序列</param>
    /// <param name="selector">选择器函数</param>
    /// <returns>包含结果或异常的 SelectTryResult 序列</returns>
    public static IEnumerable<SelectTryResult<TSource, TResult>> SelectTry<TSource, TResult>(this IEnumerable<TSource> enumerable, Func<TSource, TResult> selector)
    {
        foreach (TSource element in enumerable)
        {
            SelectTryResult<TSource, TResult> returnedValue;
            try
            {
                // 执行选择器转换
                returnedValue = new SelectTryResult<TSource, TResult>(element, selector(element), null);
            }
            catch (Exception ex)
            {
                // 捕获异常，记录到结果中
                returnedValue = new SelectTryResult<TSource, TResult>(element, default, ex);
            }
            yield return returnedValue;
        }
    }

    /// <summary>
    /// 对发生异常的转换结果执行异常处理（仅基于异常）
    /// </summary>
    /// <typeparam name="TSource">源元素类型</typeparam>
    /// <typeparam name="TResult">结果类型</typeparam>
    /// <param name="enumerable">SelectTryResult 序列</param>
    /// <param name="exceptionHandler">异常处理函数</param>
    /// <returns>处理后的结果序列</returns>
    public static IEnumerable<TResult> OnCaughtException<TSource, TResult>(this IEnumerable<SelectTryResult<TSource, TResult>> enumerable, Func<Exception, TResult> exceptionHandler)
    {
        // 若无异常则返回原始结果，否则调用异常处理器
        return enumerable.Select(x => x.CaughtException == null ? x.Result : exceptionHandler(x.CaughtException));
    }

    /// <summary>
    /// 对发生异常的转换结果执行异常处理（基于源元素和异常）
    /// </summary>
    /// <typeparam name="TSource">源元素类型</typeparam>
    /// <typeparam name="TResult">结果类型</typeparam>
    /// <param name="enumerable">SelectTryResult 序列</param>
    /// <param name="exceptionHandler">异常处理函数（包含源元素）</param>
    /// <returns>处理后的结果序列</returns>
    public static IEnumerable<TResult> OnCaughtException<TSource, TResult>(this IEnumerable<SelectTryResult<TSource, TResult>> enumerable, Func<TSource, Exception, TResult> exceptionHandler)
    {
        // 若无异常则返回原始结果，否则调用异常处理器（传入源元素）
        return enumerable.Select(x => x.CaughtException == null ? x.Result : exceptionHandler(x.Source, x.CaughtException));
    }

    /// <summary>
    /// SelectTry 操作的结果记录类，包含源元素、转换结果和可能的异常
    /// </summary>
    /// <typeparam name="TSource">源元素类型</typeparam>
    /// <typeparam name="TResult">结果类型</typeparam>
    public class SelectTryResult<TSource, TResult>
    {
        internal SelectTryResult(TSource source, TResult result, Exception exception)
        {
            Source = source;
            Result = result;
            CaughtException = exception;
        }

        /// <summary>获取源元素</summary>
        public TSource Source { get; private set; }
        /// <summary>获取转换结果（若无异常）</summary>
        public TResult Result { get; private set; }
        /// <summary>获取捕获的异常（若无异常则为 null）</summary>
        public Exception CaughtException { get; private set; }
    }
}
