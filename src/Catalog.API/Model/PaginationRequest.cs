using System.ComponentModel;

namespace eShop.Catalog.API.Model;

/// <summary>
/// 分页请求记录类，用于指定分页查询的参数
/// </summary>
/// <param name="PageSize">每页显示的记录数，默认为 10</param>
/// <param name="PageIndex">要返回的结果页索引，默认为 0</param>
public record PaginationRequest(
    [property: Description("返回的单页结果数量")]
    [property: DefaultValue(10)]
    int PageSize = 10,

    [property: Description("要返回的结果页索引")]
    [property: DefaultValue(0)]
    int PageIndex = 0
);
