namespace eShop.Ordering.API.Application.Queries;

/// <summary>
/// 订单项视图模型
/// </summary>
/// <param name="ProductName">商品名称</param>
/// <param name="Units">商品数量</param>
/// <param name="UnitPrice">单价</param>
/// <param name="PictureUrl">商品图片 URL</param>
public record Orderitem(
    string ProductName,
    int Units,
    double UnitPrice,
    string PictureUrl);

/// <summary>
/// 订单详细信息视图模型
/// </summary>
/// <param name="OrderNumber">订单编号</param>
/// <param name="Date">订单日期</param>
/// <param name="Status">订单状态</param>
/// <param name="Description">订单描述</param>
/// <param name="Street">街道地址</param>
/// <param name="City">城市</param>
/// <param name="State">州/省</param>
/// <param name="Zipcode">邮政编码</param>
/// <param name="Country">国家</param>
/// <param name="OrderItems">订单项列表</param>
/// <param name="Total">订单总金额</param>
public record Order(
    int OrderNumber,
    DateTime Date,
    string Status,
    string Description,
    string Street,
    string City,
    string State,
    string Zipcode,
    string Country,
    List<Orderitem> OrderItems,
    decimal Total);

/// <summary>
/// 订单摘要视图模型
/// </summary>
/// <param name="OrderNumber">订单编号</param>
/// <param name="Date">订单日期</param>
/// <param name="Status">订单状态</param>
/// <param name="Total">订单总金额</param>
public record OrderSummary(
    int OrderNumber,
    DateTime Date,
    string Status,
    double Total);

/// <summary>
/// 卡片类型视图模型
/// </summary>
/// <param name="Id">卡片类型 ID</param>
/// <param name="Name">卡片类型名称</param>
public record CardType(
    int Id,
    string Name);
