// 创建 Web 应用程序构建器并配置应用程序
var builder = WebApplication.CreateBuilder(args);

// 添加服务默认配置（健康检查、度量等）
builder.AddServiceDefaults();

// 注册应用程序服务（数据库、事件总线等）
builder.AddApplicationServices();

// 添加 ProblemDetails 支持（标准化的 API 错误响应格式）
builder.Services.AddProblemDetails();

// 配置 API 版本控制
// 在响应头中包含 "api-supported-versions" 和 "api-deprecated-versions"
var withApiVersioning = builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
});

// 添加默认 OpenAPI 支持
builder.AddDefaultOpenApi(withApiVersioning);

// 构建应用程序
var app = builder.Build();

// 映射默认端点（健康检查等）
app.MapDefaultEndpoints();

// 启用状态码页面中间件
app.UseStatusCodePages();

// 注册目录 API 路由
app.MapCatalogApi();

// 配置并启用 OpenAPI 文档
app.UseDefaultOpenApi();

// 启动应用程序
app.Run();
